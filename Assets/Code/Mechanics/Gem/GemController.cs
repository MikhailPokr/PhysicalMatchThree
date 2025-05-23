using UnityEngine;
using System;

namespace PMT
{
    internal class GemController : IService
    {
        private Gem _gemPrefab;

        private ScoreCounter _scoreCounter;
        private GemChainSystem _gemChainController;

        private GameObject _board; 
        
        public event Action<int> NeedGenerate;

        public GemController(
            GemChainSystem gemChainController,
            Gem gemPrefab,
            ScoreCounter scoreCounter
            )
        {
            _gemPrefab = gemPrefab;
            _scoreCounter = scoreCounter;
            _gemChainController = gemChainController;
        }

        public void GenerateBoard()
        {
            _board = new GameObject("board");
        }

        public void GenerateGem(Vector2 pos)
        {
            Gem gem = GameObject.Instantiate(_gemPrefab, pos, Quaternion.identity, _board.transform);
            gem.initialize(_gemChainController, new GemType(UnityEngine.Random.Range(0, 4)));
            gem.Click += OnGemClick;
            EventBus<GemClickEvent>.Subscribe(OnClick);
        }

        public void Generate(int count)
        {
            NeedGenerate?.Invoke(count);
        }

        private void OnClick(GemClickEvent clickEvent)
        {
            GameObject.Destroy(clickEvent.Gem.gameObject);
        }

        public void OnGemClick(Gem gem)
        {
            
        }
    }
}