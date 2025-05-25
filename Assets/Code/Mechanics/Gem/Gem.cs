using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PMT
{
    internal class Gem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GemView _view;
        public GemView View => _view;

        private IGemChainSystem _gemChainController;
        private GemType _gemType; 
        public GemType GemType => _gemType;

        private bool _clickLock;
        public void SwitchLock() => _clickLock = !_clickLock;

        public event Action<Gem> Match;
        public event Action<Gem> Dismatch;

        public void initialize(GemType type, IGemChainSystem gemChainController = null)
        {
            _gemChainController = gemChainController;
            _gemType = type;
            _view.Body.color = type.Color;

            EventBus<GameOverEvent>.Subscribe(OnGameOver);

            if (type.Effect ==  null)
            {
                _view.Outline.color = Color.black;
                return;
            }
            _view.Outline.color = type.Effect.Color;
            _gemType.Effect.ApplyInField(this);
        }
        private void OnGameOver(GameOverEvent gameOverEvent)
        {
            _clickLock = true;
        }

        public Sprite GetSprite() => _view.Body.sprite;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Gem other = collision.gameObject.GetComponent<Gem>();
            if (other != null && other._gemType.ItsSameType(_gemType))
            {
                _gemChainController?.Match(this, other);
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
                _gemChainController?.Dismatch(this, other);
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

        private void OnDestroy()
        {
            EventBus<GameOverEvent>.Unsubscribe(OnGameOver);
            _gemType.Effect?.OnDestroy();
        }

        [Serializable]
        public class GemView
        {
            public SpriteRenderer Body;
            public SpriteRenderer Outline;
        }
    }
}