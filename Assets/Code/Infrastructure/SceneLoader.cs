using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace PMT
{
    internal class SceneLoader : IService
    {
        private readonly Action<IEnumerator> _corutineRunner;

        public SceneLoader(Action<IEnumerator> corutineRunner)
        {
            _corutineRunner = corutineRunner;
        }

        public void Load(string sceneName, Action onLoad = null)
        {
            _corutineRunner?.Invoke(LoadScene(sceneName, onLoad));
        }

        private IEnumerator LoadScene(string sceneName, Action onLoad)
        {
            AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneName);

            while (!loadingScene.isDone)
                yield return null;

            onLoad?.Invoke();
        }
        public void Dispose() { }
    }
}