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
        [SerializeField] private int _spawnerCount;
        [SerializeField] private float _sidePadding = 1f; 

        private IGemController _gemController;
        private IGemChainSystem _chainSystem;
        private Palette _palette;
        private List<float> _spawnerPositions;
        private float _generationY;
        private Camera _mainCamera;

        private Coroutine _coroutine;

        public void Initialize()
        {
            _mainCamera = Camera.main;
            _gemController = ServiceLocator.Resolve<IGemController>();
            ServiceLocator.TryResolve(out _chainSystem);
            _palette = ServiceLocator.Resolve<Palette>();

            UpdateColliderSize();
            CalculateSlotPositions();
            _gemController.NeedGenerate += OnNeedGenerate;
        }

        private void UpdateColliderSize()
        {
            float cameraWidth = _mainCamera.aspect * _mainCamera.orthographicSize * 2;
            float colliderWidth = cameraWidth - (_sidePadding * 2);

            _collider.size = new Vector2(colliderWidth, _collider.size.y);
            _collider.offset = Vector2.zero;
        }

        private void CalculateSlotPositions()
        {
            _spawnerPositions = new List<float>();
            Bounds bounds = _collider.bounds;

            _generationY = bounds.center.y;

            float slotWidth = bounds.size.x / _spawnerCount;
            for (int i = 0; i < _spawnerCount; i++)
            {
                float xPos = bounds.min.x + (slotWidth * i) + (slotWidth / 2f);
                _spawnerPositions.Add(xPos);
            }
        }

        private void GenerateGem(Vector3 pos, GemType type)
        {
            Gem gem = GameObject.Instantiate(
                _palette.GemPrefabs.Find(x => x.Shape == type.Shape).Gem, pos, Quaternion.identity, _board.transform);
            Palette.RuneSprite rune = _palette.RuneSprites.FirstOrDefault(x => x.RuneType == type.Effect?.Rune);
            gem.initialize(type, rune?.Sprite);
        }

        private void OnNeedGenerate(GemType[] gems)
        {
            foreach (Transform child in _board.transform)
            {
                Destroy(child.gameObject);
            }
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Generate(gems));
        }

        IEnumerator Generate(GemType[] gems)
        {
            List<GemType> gemList = gems.ToList();
            List<int> availableSlots = Enumerable.Range(0, _spawnerPositions.Count).ToList();
            List<int> lastUsedSlots = new();
            int slotCooldown = availableSlots.Count / 2;

            for (int i = 0; i < gems.Length; i++)
            {
                availableSlots.RemoveAll(x => lastUsedSlots.Contains(x));

                int randomSlotIndex = Random.Range(0, availableSlots.Count);
                int slotIndex = availableSlots[randomSlotIndex];

                availableSlots.AddRange(lastUsedSlots);
                if (lastUsedSlots.Count > slotCooldown)
                    lastUsedSlots.RemoveAt(0);
                lastUsedSlots.Add(slotIndex);

                float xPos = _spawnerPositions[slotIndex];
                Vector2 spawnPosition = new Vector2(xPos, _generationY);

                GemType gem = gemList[Random.Range(0, gemList.Count)];
                GenerateGem(spawnPosition, gem);

                gemList.Remove(gem);
                yield return new WaitForSeconds(_time / 1000f);
            }

            _coroutine = null;
        }

        private void OnDestroy()
        {
            _gemController.NeedGenerate -= OnNeedGenerate;
        }

        private void OnDrawGizmos()
        {
            if (_collider == null || Camera.main == null) return;

            float cameraWidth = Camera.main.aspect * Camera.main.orthographicSize * 2;
            float colliderWidth = cameraWidth - (_sidePadding * 2);
            Vector2 newSize = new Vector2(colliderWidth, _collider.size.y);

            Vector3 center = _collider.transform.position;
            Bounds newBounds = new Bounds(center, newSize);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(newBounds.center, newBounds.size);

            float slotWidth = newSize.x / _spawnerCount;
            Gizmos.color = Color.cyan;
            for (int i = 0; i < _spawnerCount; i++)
            {
                float xPos = newBounds.min.x + (slotWidth * i) + (slotWidth / 2f);
                Vector3 position = new Vector3(xPos, center.y, 0);

                Gizmos.DrawSphere(position, 0.2f);
                Gizmos.DrawLine(position - Vector3.up * 0.3f, position + Vector3.up * 0.3f);
                Gizmos.DrawLine(position - Vector3.right * 0.3f, position + Vector3.right * 0.3f);
            }
        }
    }
}