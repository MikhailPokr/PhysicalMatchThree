using System;
using System.Collections.Generic;
using UnityEngine;

namespace PMT
{
    [CreateAssetMenu(fileName = "Palette", menuName = "Game/Palette")]
    internal class Palette : ScriptableObject, IService
    {
        public List<GemPrefabData> GemPrefabs;
        public GameOverAlert GameOverAlertPrefab;

        [Serializable]
        public class GemPrefabData
        {
            public Shape Shape;
            public Gem Gem;
        }
    }
}