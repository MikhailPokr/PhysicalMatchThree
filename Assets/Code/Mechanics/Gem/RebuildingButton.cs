using UnityEngine;

namespace PMT
{
    internal class RebuildingButton : MonoBehaviour, IInitializable
    {
        private IGemController _gemController;
        private bool _locked;
        public void Initialize()
        {
            _gemController = ServiceLocator.Resolve<IGemController>();
            EventBus<GameOverEvent>.Subscribe(OnGameOver);
        }
        private void OnGameOver(GameOverEvent gameOverEvent)
        {
            _locked = true;
        }
        public void Click()
        {
            if (_locked)
                return;
            _gemController.Generate();
        }

        private void OnDestroy()
        {
            EventBus<GameOverEvent>.Unsubscribe(OnGameOver);
        }
    }
}