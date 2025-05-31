using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PMT
{
    internal class GemController : IGemController
    {
        public event Action<GemType[]> NeedGenerate;
        public event Action FieldClear;

        private List<Color> _colors;
        private List<Shape> _shapes;
        int _copiesCount;
        float _effectChance;

        private int _count;

        public GemController(List<Color> colors, List<Shape> shapes, int copiesCount, float effectChance)
        {
            _colors = colors;
            _shapes = shapes;
            _copiesCount = copiesCount;
            _effectChance = effectChance;

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
            if (count > _colors.Count * _shapes.Count * _copiesCount)
                throw new Exception("Много");

            List<GemType> list = new();

            List<(Shape shape, Color color)> allCombinations = new();
            foreach (Shape shape in _shapes)
            {
                foreach (Color color in _colors)
                {
                    allCombinations.Add((shape, color));
                }
            }

            List<(Shape shape, Color color)> shuffledCombinations = allCombinations.OrderBy(x => UnityEngine.Random.value).ToList();

            int neededCombinations = (int)Math.Ceiling((double)count / _copiesCount);
            List<(Shape shape, Color color)> selectedCombinations = shuffledCombinations.Take(neededCombinations).ToList();

            foreach ((Shape shape, Color color) in selectedCombinations)
            {
                int copiesToAdd = Math.Min(_copiesCount, count - list.Count);
                for (int j = 0; j < copiesToAdd; j++)
                {
                    list.Add(new GemType(shape, color, GetEffect(UnityEngine.Random.value)));
                }
                if (list.Count >= count) break;
            }

            return list.ToArray();
        }

        private BaseGemSpecialEffect GetEffect(float rValue)
        {
            if (rValue < _effectChance)
                return new HeavyGemEffect();
            if (rValue < _effectChance * 2)
                return new StickyGemEffect();
            if (rValue < _effectChance * 3)
                return new FrozenGemEffect();
            if (rValue < _effectChance * 4)
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

        public void Dispose()
        {
            EventBus<GemClickEvent>.Unsubscribe(OnClick);
        }
    }
}