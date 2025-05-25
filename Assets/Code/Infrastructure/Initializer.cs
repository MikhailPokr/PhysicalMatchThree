using UnityEngine;

namespace PMT
{
    internal class Initializer : IService
    {
        public void InitializeObjects()
        {
            var monobehObjects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < monobehObjects.Length; i++)
            {
                if (monobehObjects[i] is IInitializable initializable)
                {
                    initializable.Initialize();
                }
            }
        }
        public void Dispose() { }
    }
}