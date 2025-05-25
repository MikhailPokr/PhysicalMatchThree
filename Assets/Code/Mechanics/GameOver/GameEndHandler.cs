using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PMT
{
    internal class GameEndHandler : IGameEndHandler
    {
        private IGemController _gemController;
        private IActionBarController _actionBarController;

        private GameOverAlert _alertPrefab;
        private Canvas _canvas;

        public GameEndHandler(
            IGemController gemController,
            IActionBarController actionBarController,
            GameOverAlert alertPrefab,
            Canvas canvas) 
        {
            _gemController = gemController;
            _alertPrefab = alertPrefab;
            _actionBarController = actionBarController;
            _canvas = canvas;

            _gemController.FieldClear += FieldClear;
            _actionBarController.ActionBarFull += OnActionBarFull;
        }

        private void OnActionBarFull()
        {
            GameOverAlert alert = GameObject.Instantiate(_alertPrefab, _canvas.transform);
            alert.Initialize("Поражение");
            EventBus<GameOverEvent>.Publish(new GameOverEvent());
        }

        private void FieldClear()
        {
            GameOverAlert alert = GameObject.Instantiate(_alertPrefab, _canvas.transform);
            alert.Initialize("Победа");
            EventBus<GameOverEvent>.Publish(new GameOverEvent());
        }

        public void Dispose()
        {
            _gemController.FieldClear -= FieldClear;
            _actionBarController.ActionBarFull -= OnActionBarFull;
        }
    }
}
