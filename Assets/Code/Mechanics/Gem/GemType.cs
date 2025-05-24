using UnityEngine;
using System; 

namespace PMT
{
    internal struct GemType : IEquatable<GemType>
    {
        public bool IsNull;
        private Shape _shape;
        public Shape Shape => _shape;
        private Color _color;
        public Color Color => _color;

        public static GemType Default => new GemType() { IsNull = true };
        public GemType(Shape shape, Color color)
        {
            IsNull = false;
            _shape = shape;
            _color = color;
        }

        public bool Equals(GemType other)
        {
            return
                IsNull == other.IsNull &&
                _shape == other._shape &&
                _color == other._color;
        }

        public override bool Equals(object obj)
        {
            return obj is GemType other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_shape, IsNull);
        }

        #region operators
        public static bool operator ==(GemType left, GemType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GemType left, GemType right)
        {
            return !(left == right);
        }
    }
    #endregion
}