using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PMT
{
    internal class GameEndHandler : IService
    {
        private GameOverAlert _alertPrefab;
        private ActionBarController _actionBarController;
        private GemController _gemController;
        public GameEndHandler(GemController gemController, ActionBarController actionBarController, GameOverAlert alertPrefab) 
        {
            _gemController = gemController;
            _alertPrefab = alertPrefab;
            _actionBarController = actionBarController;

            _gemController.FieldClear += FieldClear;
            _actionBarController.ActionBarFull += OnActionBarFull;
        }

        private void OnActionBarFull()
        {
            Canvas canvas = GameObject.FindAnyObjectByType<Canvas>();
            GameOverAlert alert = GameObject.Instantiate(_alertPrefab, canvas.transform);
            alert.Initialize("Поражение");
        }

        private void FieldClear()
        {
            Canvas canvas = GameObject.FindAnyObjectByType<Canvas>();
            GameOverAlert alert = GameObject.Instantiate(_alertPrefab, canvas.transform);
            alert.Initialize("Победа");
        }
    }
}
