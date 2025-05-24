using UnityEngine;

namespace PMT
{
    internal class GameCore : MonoBehaviour
    {
        [SerializeField] private Palette _palette;

        private GemChainSystem _gemChainSystem;
        private GemGenerator _gemGenerator;
        private ScoreCounter _scoreCounter;
        private GemController _gemController;
        private ActionBarController _actionBarController;
        private GameEndHandler _gameEndHandler;
        private Initializer _initializer;

        void Start()
        {
            ServiceLocator.Register(_palette);

            _gemChainSystem = ServiceLocator.Register(new GemChainSystem());
            _gemController = ServiceLocator.Register(new GemController());
            _scoreCounter = ServiceLocator.Register(new ScoreCounter());
            _actionBarController = ServiceLocator.Register(new ActionBarController(7, new MatchRule(3)));
            _gameEndHandler = ServiceLocator.Register(new GameEndHandler(_gemController, _actionBarController, _palette.GameOverAlertPrefab));

            _initializer = ServiceLocator.Register(new Initializer());

            _initializer.InitializeObjects();

            _gemController.Generate(105);
        }

        void Update()
        {
        }
    }
}