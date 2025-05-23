using UnityEngine;

namespace PMT
{
    internal class GemType
    {
        private int _type;

        public GemType(int type)
        {
            _type = type;
        }

        public bool IsSameType(GemType other) => _type == other._type;

        public Color GetColor()
        {
            switch (_type)
            { 
                case 0:
                    return Color.red;
                case 1:
                    return Color.green;
                case 2:
                    return Color.blue;
                case 3:
                    return Color.yellow;
            }
            return Color.white;
        }
    }
}