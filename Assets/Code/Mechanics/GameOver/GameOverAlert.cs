using TMPro;
using UnityEngine;

namespace PMT
{
    internal class GameOverAlert : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void Initialize(string text)
        {
            _text.text = text;
        }
    }
}