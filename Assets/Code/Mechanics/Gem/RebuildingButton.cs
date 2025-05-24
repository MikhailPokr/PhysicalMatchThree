using UnityEngine;

namespace PMT
{
    internal class RebuildingButton : MonoBehaviour, IInitializable
    {
        private GemController _gemController;
        public void Initialize()
        {
            _gemController = ServiceLocator.Resolve<GemController>();
        }

        public void Click()
        {
            _gemController.Generate();
        }

    }
}