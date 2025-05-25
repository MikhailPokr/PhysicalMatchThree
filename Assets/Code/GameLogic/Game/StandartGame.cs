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
                new List<Color> { Color.red, Color.blue, Color.green, Color.cyan, Color.yellow, Color.magenta},
                new List<Shape> { Shape.Circle, Shape.Square, Shape.Heart, Shape.Triangle, Shape.Star, Shape.Rhomb},
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