using UnityEngine;

namespace PMT
{
    internal class GameCore : MonoBehaviour
    {
        [SerializeField] private Gem _gemPrefab;

        private GemChainSystem _gemChainSystem;
        private GemGenerator _gemGenerator;
        private ScoreCounter _scoreCounter;
        private GemController _gemController;
        private ActionBarController _actionBarController;
        private Initializer _initializer;

        void Start()
        {
            _gemChainSystem = ServiceLocator.Register(new GemChainSystem());
            _gemController = ServiceLocator.Register(new GemController(_gemChainSystem, _gemPrefab, _scoreCounter));
            _scoreCounter = ServiceLocator.Register(new ScoreCounter());
            _actionBarController = ServiceLocator.Register(new ActionBarController(7, new MatchRule(3)));
            _initializer = ServiceLocator.Register(new Initializer());

            _initializer.InitializeObjects();

            _gemController.GenerateBoard();
            _gemController.Generate(20);
        }

        void Update()
        {
        }
    }
}