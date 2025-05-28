using System;
using System.Collections.Generic;
using UnityEngine;

namespace PMT
{
    [CreateAssetMenu(fileName = "Palette", menuName = "Game/Palette")]
    internal class Palette : ScriptableObject, IService
    {
        public List<GemPrefabData> GemPrefabs;
        public List<RuneSprite> RuneSprites;
        public GameOverAlert GameOverAlertPrefab;
        public void Dispose() { }

        [Serializable]
        public class GemPrefabData
        {
            public Shape Shape;
            public Gem Gem;
        }

        [Serializable]
        public class RuneSprite
        {
            public RuneType RuneType;
            public Sprite Sprite;
        }
    }
}