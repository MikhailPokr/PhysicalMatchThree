using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PMT
{
    internal class Gem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _view;
        public SpriteRenderer View => _view;

        private GemChainSystem _gemChainController;
        private GemType _gemType; 
        public GemType GemType => _gemType;


        public void initialize(GemChainSystem gemChainController, GemType type)
        {
            _gemChainController = gemChainController;
            _gemType = type;
            _view.color = type.Color;
        }

        public Sprite GetSprite() => _view.sprite;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Gem other = collision.gameObject.GetComponent<Gem>();
            if (other != null && other._gemType == _gemType)
            {
                _gemChainController.Match(this, other);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Gem other = collision.gameObject.GetComponent<Gem>();
            if (other != null && other._gemType == _gemType)
            {
                _gemChainController.Dismatch(this, other);
            }
        }

        public void OnMouseDown() => EventBus<GemClickEvent>.Publish(new GemClickEvent(this));
    }
}