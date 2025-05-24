using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PMT
{
    internal class GemController : IService
    {        
        public event Action<GemType[]> NeedGenerate;
        public event Action FieldClear;

        private int _count;
       
        public GemController()
        {
            EventBus<GemClickEvent>.Subscribe(OnClick);
        }

        public void Generate() => Generate(_count);
        public void Generate(int count)
        {
            _count = count;
            NeedGenerate?.Invoke(GetGemList(count));
        }

        private GemType[] GetGemList(int count)
        {
            List<Color> colors = new List<Color>() { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.gray };
            List<Shape> shapes = Enum.GetValues(typeof(Shape)).Cast<Shape>().ToList();

            if (count > shapes.Count * colors.Count * 3)
                throw new Exception("Много");

            List<GemType> list = new List<GemType>();

            int fullCombinations = count / 3;
            int remaining = count % 3;

            for (int i = 0; i < fullCombinations; i++)
            {
                int colorIndex = i / shapes.Count;
                int shapeIndex = i % shapes.Count;

                for (int j = 0; j < 3; j++)
                {
                    if (list.Count >= count) break;
                    list.Add(new GemType(shapes[shapeIndex], colors[colorIndex], GetEffect()));
                }
            }

            if (remaining > 0 && fullCombinations < shapes.Count * colors.Count)
            {
                int colorIndex = fullCombinations / shapes.Count;
                int shapeIndex = fullCombinations % shapes.Count;

                for (int j = 0; j < remaining; j++)
                {
                    list.Add(new GemType(shapes[shapeIndex], colors[colorIndex], GetEffect()));
                }
            }

            return list.ToArray();
        }

        private BaseGemSpecialEffect GetEffect()
        {
            float value = UnityEngine.Random.value;
            if (value < 0.01)
                return new HeavyGemEffect();
            if (value < 0.02)
                return new StickyGemEffect();
            if (value < 0.03)
                return new FrozenGemEffect();
            if (value < 0.04)
                return new BombGemEffect();
            return null;
        }

        private void OnClick(GemClickEvent clickEvent)
        {
            _count--;
            GameObject.Destroy(clickEvent.Gem.gameObject);

            if (_count == 0)
            {
                FieldClear?.Invoke();
            }
        }
    }
}