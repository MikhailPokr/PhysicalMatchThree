using System;
using UnityEngine;

namespace PMT
{
    internal class ScoreCounter : IService
    {
        private int _score;

        public static event Action<int> ScoreChanged;
        public ScoreCounter()
        {
            _score = 0;
        }

        public void AddScore()
        {
            _score++;
            ScoreChanged?.Invoke(_score);
        }
    }
}