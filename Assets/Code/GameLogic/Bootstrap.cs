using System;
using System.Collections;
using UnityEngine;

namespace PMT
{
    internal class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Palette _palette;
        [SerializeField] private GameMode _mode;

        private SceneLoader _loader;
        private IGameProcess _game;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            ServiceLocator.Register(_palette);
            _loader = ServiceLocator.Register(new SceneLoader(RunCoroutine));
            _game = new GameProcess(_palette, _loader, _mode);

            _loader.Load("Game", _game.Start);
        }

        public void RunCoroutine(IEnumerator coroutineAction)
        {
            StartCoroutine(coroutineAction);
        }

        private void Update()
        {
            _game.Update();
        }
    }
}
