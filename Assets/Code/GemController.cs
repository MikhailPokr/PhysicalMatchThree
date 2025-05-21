using UnityEngine;

public class GemController : MonoBehaviour
{
    [SerializeField] private GemView _gemPrefab;

    private ScoreCounter _scoreCounter;
    private GameObject _board;

    private void Start()
    {
        _board = new GameObject("board");
        _scoreCounter = new ScoreCounter();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            GemView gem = Instantiate(_gemPrefab, pos, Quaternion.identity, _board.transform);
            gem.MatchThree += OnMatchThree;
        }
    }

    private void OnMatchThree(System.Collections.Generic.List<GemView> gemsNear)
    {
        for (int i = 0; i < gemsNear.Count; i++)
        {
            Destroy(gemsNear[i].gameObject);
        }
        _scoreCounter.AddScore();
    }
}
