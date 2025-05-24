using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PMT
{
    internal class Gem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GemView _view;
        public GemView View => _view;

        private GemChainSystem _gemChainController;
        private GemType _gemType; 
        public GemType GemType => _gemType;

        private bool _clickLock;
        public void SwitchLock() => _clickLock = !_clickLock;

        public event Action<Gem> Match;
        public event Action<Gem> Dismatch;


        public void initialize(GemChainSystem gemChainController, GemType type)
        {
            _gemChainController = gemChainController;
            _gemType = type;
            _view.Body.color = type.Color;

            if (type.Effect ==  null)
            {
                _view.Outline.color = Color.black;
                return;
            }
            _view.Outline.color = type.Effect.Color;
            _gemType.Effect.ApplyInField(this);
        }

        public Sprite GetSprite() => _view.Body.sprite;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Gem other = collision.gameObject.GetComponent<Gem>();
            if (other != null && other._gemType.ItsSameType(_gemType))
            {
                _gemChainController.Match(this, other);
            }
            if (other != null)
            {
                Match?.Invoke(other);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Gem other = collision.gameObject.GetComponent<Gem>();
            if (other != null && other._gemType.ItsSameType(_gemType))
            {
                _gemChainController.Dismatch(this, other);
            }
            if (other != null)
            {
                Dismatch?.Invoke(other);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_clickLock)
                return;
            EventBus<GemClickEvent>.Publish(new GemClickEvent(this));
        }

        [Serializable]
        public class GemView
        {
            public SpriteRenderer Body;
            public SpriteRenderer Outline;
        }
    }
}