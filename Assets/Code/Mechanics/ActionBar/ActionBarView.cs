using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PMT
{
    public class ActionBarView : MonoBehaviour, IInitializable
    {
        [SerializeField] private Image[] _slotImages;

        private ActionBarController _actionBarController;


        public void Initialize()
        {
            _actionBarController = ServiceLocator.Resolve<ActionBarController>();

            _actionBarController.ActionBarChanged += OnActionBarChanged;
        }

        private void OnActionBarChanged(GemType[] slots)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null)
                {
                    _slotImages[i].color = slots[i].GetColor();
                }
                else
                {
                    _slotImages[i].color = Color.white;
                }
                
            }
                
        }
    }
}