using System;
using System.Collections;
using UnityEngine;

namespace PMT
{
    internal class GemGenerator : MonoBehaviour, IInitializable
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private float _time;

        private GemController _gemController;
        public void Initialize()
        {
            _gemController = ServiceLocator.Resolve<GemController>();

            _gemController.NeedGenerate += OnNeedGenerate;
        }

        private void OnNeedGenerate(int count)
        {
            StartCoroutine(Generate(count));
        }

        IEnumerator Generate(int count)
        {
            int gemCreated = 0;

            while (gemCreated < count)
            {
                Vector2 randomPoint = GetRandomPointInCollider();

                _gemController.GenerateGem(randomPoint);

                gemCreated++;

                yield return new WaitForSeconds(_time / 1000f);
            }
        }
        Vector2 GetRandomPointInCollider()
        {
            Bounds bounds = _collider.bounds;

            float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

            return new Vector2(randomX, randomY);
        }
    }
}