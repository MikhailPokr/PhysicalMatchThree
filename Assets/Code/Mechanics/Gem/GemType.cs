using UnityEngine;
using System;

namespace PMT
{
    internal class GemType
    {
        private Shape _shape;
        public Shape Shape => _shape;
        private Color _color;
        public Color Color => _color;
        private BaseGemSpecialEffect _effect;
        public BaseGemSpecialEffect Effect => _effect;

        public GemType(Shape shape, Color color, BaseGemSpecialEffect effect = null)
        {
            _shape = shape;
            _color = color;
            _effect = effect;
        }

        public bool ItsSameType(GemType other)
        {
            if (other == null) return false;
            if (
                _shape == other._shape &&
                _color == other._color)
            return true;
            return false;
        }

    }
}