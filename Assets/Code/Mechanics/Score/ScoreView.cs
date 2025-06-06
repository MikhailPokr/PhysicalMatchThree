using TMPro;
using UnityEngine;

namespace PMT
{
    internal class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            ScoreCounter.ScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged(int score)
        {
            UpdateData(score);
        }

        public void UpdateData(int score)
        {
            _text.text = score.ToString();
        }

        private void OnDestroy()
        {
            ScoreCounter.ScoreChanged -= OnScoreChanged;
        }
    }
}