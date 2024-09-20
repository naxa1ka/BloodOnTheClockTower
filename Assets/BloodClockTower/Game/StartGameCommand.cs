using System;
using Cysharp.Threading.Tasks;
using Nxlk.UIToolkit;
using UnityEngine;

namespace BloodClockTower.Game
{
    public class StartGameCommand
    {
        private readonly ViewFactory<PlayerIconView> _playerIconViewFactory;
        private readonly Action<GameScopeMono> _onLoad;

        public StartGameCommand(
            ViewFactory<PlayerIconView> playerIconViewFactory,
            Action<GameScopeMono> onLoad
        )
        {
            _playerIconViewFactory = playerIconViewFactory;
            _onLoad = onLoad;
        }

        public async UniTask Execute(int playersAmount)
        {
            var gameScene = new GameScene();
            await gameScene.Load();

            var safetyUiDocument = gameScene.Context.UIDocument.ToSafetyUiDocument();
            var gameObject = new GameObject("GameScope");
            var gameScopeMono = gameObject.AddComponent<GameScopeMono>();
            gameScopeMono.Construct(_playerIconViewFactory, safetyUiDocument, playersAmount);
            _onLoad.Invoke(gameScopeMono);
        }
    }
}
