using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PMT
{
    internal class GemGenerator : MonoBehaviour, IInitializable
    {
        [SerializeField] private GameObject _board;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private float _time;

        private GemController _gemController;
        private GemChainSystem _chainSystem;
        private Palette _palette;


        public void Initialize()
        {
            _gemController = ServiceLocator.Resolve<GemController>();
            _chainSystem = ServiceLocator.Resolve<GemChainSystem>();
            _palette = ServiceLocator.Resolve<Palette>();

            _gemController.NeedGenerate += OnNeedGenerate;
        }

        private void GenerateGem(Vector3 pos, GemType type)
        {
            Gem gem = GameObject.Instantiate(_palette.GemPrefabs.Find(x => x.Shape == type.Shape).Gem, pos, Quaternion.identity, _board.transform);
            gem.initialize(_chainSystem, type);
        }

        private void OnNeedGenerate(GemType[] gems)
        {
            foreach (Transform child in _board.transform)
            {
                Destroy(child.gameObject);
            }
            StartCoroutine(Generate(gems));
        }

        IEnumerator Generate(GemType[] gems)
        {
            List<GemType> gemList = gems.ToList();

            for (int i = 0; i < gems.Length; i++)
            {
                Vector2 randomPoint = GetRandomPointInCollider();
                GemType gem = gemList[Random.Range(0, gemList.Count)];

                GenerateGem(randomPoint, gem);

                gemList.Remove(gem);

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