using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PMT
{
    internal class GameOverAlert : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void Initialize(string text)
        {
            _text.text = text;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventBus<RestartEvent>.Publish(new RestartEvent());
            Destroy(gameObject);
        }
    }
}