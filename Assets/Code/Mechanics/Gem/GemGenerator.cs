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
        [SerializeField] private int _slotsCount = 5; 

        private GemController _gemController;
        private GemChainSystem _chainSystem;
        private Palette _palette;
        private List<float> _slotPositions;
        private float _generationY;

        public void Initialize()
        {
            _gemController = ServiceLocator.Resolve<GemController>();
            _chainSystem = ServiceLocator.Resolve<GemChainSystem>();
            _palette = ServiceLocator.Resolve<Palette>();

            CalculateSlotPositions();
            _gemController.NeedGenerate += OnNeedGenerate;
        }

        private void CalculateSlotPositions()
        {
            _slotPositions = new List<float>();
            Bounds bounds = _collider.bounds;

            _generationY = bounds.center.y;

            float slotWidth = bounds.size.x / _slotsCount;
            for (int i = 0; i < _slotsCount; i++)
            {
                float xPos = bounds.min.x + (slotWidth * i) + (slotWidth / 2f);
                _slotPositions.Add(xPos);
            }
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
            List<int> availableSlots = Enumerable.Range(0, _slotPositions.Count).ToList();

            for (int i = 0; i < gems.Length; i++)
            {
                if (availableSlots.Count == 0)
                {
                    availableSlots = Enumerable.Range(0, _slotPositions.Count).ToList();
                }

                // ¬ыбираем случайный слот из доступных
                int randomSlotIndex = Random.Range(0, availableSlots.Count);
                int slotIndex = availableSlots[randomSlotIndex];
                availableSlots.RemoveAt(randomSlotIndex);

                // ѕолучаем позицию слота
                float xPos = _slotPositions[slotIndex];
                Vector2 spawnPosition = new Vector2(xPos, _generationY);

                // ¬ыбираем случайный гем из доступных
                GemType gem = gemList[Random.Range(0, gemList.Count)];

                GenerateGem(spawnPosition, gem);

                gemList.Remove(gem);

                yield return new WaitForSeconds(_time / 1000f);
            }
        }

        // ћетод дл€ отображени€ позиций слотов в гизмосах
        private void OnDrawGizmos()
        {
            if (_collider == null) return;

            CalculateSlotPositions();

            Gizmos.color = Color.green;
            foreach (float xPos in _slotPositions)
            {
                Vector3 position = new Vector3(xPos, _generationY, 0);
                Gizmos.DrawSphere(position, 0.2f);
                Gizmos.DrawLine(position - Vector3.up * 0.3f, position + Vector3.up * 0.3f);
                Gizmos.DrawLine(position - Vector3.right * 0.3f, position + Vector3.right * 0.3f);
            }
        }
    }
}