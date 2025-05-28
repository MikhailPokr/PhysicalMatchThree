using PMT;
using System.Collections.Generic;
using UnityEngine;

internal class StandartGame : BaseGame
{
    private Palette _palette;
    private SceneLoader _sceneLoader;
    private Initializer _initializer;

    IGemController _gemController;
    IActionBarController _actionBarController;
    IGameEndHandler _gameEndHandler;

    public StandartGame(Palette palette, SceneLoader sceneLoader, Initializer initializer, int slots, int matchRule, int gemCount, float effectChance)
    {
        _palette = palette;
        _sceneLoader = sceneLoader;
        _initializer = initializer;

        _gemController = ServiceLocator.Register<IGemController>(
            new GemController(
                new List<Color> { new(0.87f, 0.98f, 0.43f), new(0.98f, 0.43f, 0.47f), new(0.43f, 0.45f, 0.98f), new(0.43f, 0.98f, 0.52f), new(0.98f, 0.43f, 0.96f), new(0.43f, 0.92f, 0.98f) },
                new List<Shape> { Shape.Circle, Shape.Square, Shape.Heart, Shape.Triangle, Shape.Pentagon, Shape.Rhomb},
                matchRule,
                effectChance
                ));

        _actionBarController = ServiceLocator.Register<IActionBarController>(
            new ActionBarController(
               slots,
                new MatchRule(matchRule)
                ));

        Canvas canvas = GameObject.FindAnyObjectByType<Canvas>();
        _gameEndHandler = ServiceLocator.Register<IGameEndHandler>(
            new GameEndHandler(
                _gemController,
                _actionBarController,
                _palette.GameOverAlertPrefab,
                canvas
                ));

        _initializer.InitializeObjects();

        _gemController.Generate(gemCount);
    }
}