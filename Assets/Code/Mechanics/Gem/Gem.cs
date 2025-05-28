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

        public void initialize(GemType type, Sprite runeSprite)
        {
            ServiceLocator.TryResolve<IGemChainSystem>(out _gemChainController);
            _gemType = type;
            _view.Body.color = type.Color;

            EventBus<GameOverEvent>.Subscribe(OnGameOver);

            if (type.Effect ==  null)
            {
                _view.Rune.color = new Color(0, 0, 0, 0);
                return;
            }
            _view.Rune.color = type.Color;
            _view.Rune.sprite = runeSprite;
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
            if (other == null)
                return;
            EventBus<MatchEvent>.Publish(new MatchEvent(true, this, other));
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Gem other = collision.gameObject.GetComponent<Gem>();
            if (other == null)
                return;
            EventBus<MatchEvent>.Publish(new MatchEvent(true, this, other));
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
            _gemType?.Effect?.OnDestroy();
        }

        [Serializable]
        public class GemView
        {
            public SpriteRenderer Body;
            public SpriteRenderer Rune;
        }
    }
}