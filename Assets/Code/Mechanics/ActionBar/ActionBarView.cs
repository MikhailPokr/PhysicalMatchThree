using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PMT
{
    public class ActionBarView : MonoBehaviour, IInitializable
    {
        [SerializeField] private Image[] _slotImages;
        [SerializeField] private Image[] _slotsBG;

        private IActionBarController _actionBarController;
        private Palette _palette;

        public void Initialize()
        {
            _actionBarController = ServiceLocator.Resolve<IActionBarController>();
            _palette = ServiceLocator.Resolve<Palette>();

            _actionBarController.ActionBarChanged += OnActionBarChanged;
        }

        private void OnActionBarChanged(GemType[] slots)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    _slotImages[i].sprite = _palette.GemPrefabs.Find(x => x.Shape == slots[i].Shape).Gem.GetSprite();
                    _slotImages[i].color = slots[i].Color;
                    if (slots[i].Effect == null)
                        _slotsBG[i].color = Color.black;
                    else
                        _slotsBG[i].color = slots[i].Effect.Color;
                }
                else
                {
                    Color color = new(0, 0, 0, 0);
                    _slotImages[i].color = color;
                    _slotsBG[i].color = Color.black;
                }
                
            }
                
        }

        private void OnDestroy()
        {
            _actionBarController.ActionBarChanged -= OnActionBarChanged;
        }
    }
}