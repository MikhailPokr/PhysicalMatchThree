using UnityEngine;

namespace PMT
{
    internal class GameProcess : IGameProcess
    {
        private GameMode _mode;
        private Palette _palette;
        public SceneLoader _sceneLoader;

        private BaseGame _game;

        private Initializer _initializer;

        public GameProcess(
            Palette palette, 
            SceneLoader sceneLoader,
            GameMode gameMode) 
        {
            _palette = palette;
            _sceneLoader = sceneLoader;
            _mode = gameMode;

            EventBus<RestartEvent>.Subscribe(Restart);
        }

        public void Start()
        {
            _initializer = ServiceLocator.Register(new Initializer());

            switch (_mode)
            {
                case GameMode.Standart:
                    {
                        SetStandartGame();
                        return;
                    }
                default: return;
            } 
        }

        public void Update()
        {
            if ( _game != null )
                _game.Update();
        }

        private void Restart(RestartEvent restartData)
        {
            _sceneLoader.Load("Game", Start);
        }

        private void SetStandartGame()
        {
            _game = new StandartGame(
                _palette,
                _sceneLoader,
                _initializer,
                7, //������
                3, //���������� �� ���� ��� ��������
                105, //����� �� ����
                0.01f //���� ������� �������
                );
        }

        private void SetSecondGame()
        {
            //_game = new SecondGame();
            // ������:
            /*
            IGemChainSystem gemChainSystem = ServiceLocator.Register<IGemChainSystem>(new GemChainSystem());
            IScoreCounter scoreCounter = ServiceLocator.Register<IScoreCounter>(new ScoreCounter());
            */
        }
    }
}