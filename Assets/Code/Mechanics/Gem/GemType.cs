﻿using UnityEngine;
using System;
using System.Collections.Generic;

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

        private bool ItsSameType(GemType other)
        {
            if (other == null) 
                return false;
            if (
                _shape == other._shape &&
                _color == other._color)
            {
                return true;
            }
            return false;
        }

        public static bool operator ==(GemType gem1, GemType gem2)
        {
            if (ReferenceEquals(gem1, gem2))
                return true;
            if (gem1 is null || gem2 is null)
                return false;
            return gem1.ItsSameType(gem2) || gem2.ItsSameType(gem1);
        }
        public static bool operator !=(GemType gem1, GemType gem2) => !(gem1 == gem2);



        public override int GetHashCode()
        {
            return HashCode.Combine(_shape, _color, _effect);
        }

        public override bool Equals(object obj)
        {
            return obj is GemType type &&
                   _shape == type._shape &&
                   _color.Equals(type._color) &&
                   EqualityComparer<BaseGemSpecialEffect>.Default.Equals(_effect, type._effect);
        }
    }
}