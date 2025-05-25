using System;
using UnityEngine;

namespace PMT
{
    internal class ScoreCounter : IScoreCounter
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

        public void Dispose() { }
    }
}