using UnityEngine;

namespace PMT
{
    internal class BoarderGenerator : MonoBehaviour, IInitializable
    {
        [SerializeField] private float sideOffset;
        [SerializeField] private float bottomOffset;
        [SerializeField] private float colliderThickness;

        private Camera mainCamera;
        private BoxCollider2D[] boundaryColliders;

        public void Initialize()
        {
            Generate();
        }

        private void Generate()
        {
            mainCamera = Camera.main;

            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = mainCamera.aspect * mainCamera.orthographicSize * 2;

            boundaryColliders = new BoxCollider2D[3];

            boundaryColliders[0] = GenerateBoarder("LeftBorder",
                new Vector2(colliderThickness, cameraHeight),
                new Vector2(-cameraWidth / 2 - sideOffset - colliderThickness / 2, 0));

            boundaryColliders[1] = GenerateBoarder("RightBorder",
                new Vector2(colliderThickness, cameraHeight),
                new Vector2(cameraWidth / 2 + sideOffset + colliderThickness / 2, 0));

            boundaryColliders[2] = GenerateBoarder("BottomBorder",
                new Vector2(cameraWidth + (sideOffset + colliderThickness) * 2, colliderThickness),
                new Vector2(0, -cameraHeight / 2 - bottomOffset - colliderThickness / 2));
        }

        private BoxCollider2D GenerateBoarder(string name, Vector2 size, Vector2 position)
        {
            GameObject borderObject = new GameObject(name);
            borderObject.transform.SetParent(transform);
            borderObject.transform.position = position;

            BoxCollider2D border = borderObject.AddComponent<BoxCollider2D>();
            border.size = size;

            return border;
        }
    }
}