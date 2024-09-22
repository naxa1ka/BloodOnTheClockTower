using System;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using OneOf;
using OneOf.Types;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BloodClockTower.Game
{
    public class GameScope : DisposableObject, IInitializable
    {
        private readonly Game _game;
        private readonly ViewFactory<PlayerIconView> _playerIconViewFactory;
        private readonly SafetyUiDocument _safetyUiDocument;
        private readonly UIToolkitEventSystem _uiToolkitEventSystem;
        private OneOf<NightScopeMono, None> _nightScope = new None();

        public GameScope(
            ViewFactory<PlayerIconView> playerIconViewFactory,
            SafetyUiDocument safetyUiDocument,
            UIToolkitEventSystem uiToolkitEventSystem,
            int playersAmount
        )
        {
            _safetyUiDocument = safetyUiDocument;
            _playerIconViewFactory = playerIconViewFactory;
            _uiToolkitEventSystem = uiToolkitEventSystem;
            _game = new Game(playersAmount).AddTo(disposables);

            Disposable.Create(DisposeNight).AddTo(disposables);
        }

        public void Initialize()
        {
            _game
                .CurrentNight.Subscribe(night =>
                {
                    DisposeNight();

                    var gameObject = new GameObject("NightScope");
                    var nightScopeMono = gameObject.AddComponent<NightScopeMono>();
                    nightScopeMono.Construct(
                        _playerIconViewFactory,
                        _safetyUiDocument,
                        _game,
                        night,
                        _uiToolkitEventSystem
                    );

                    _nightScope = nightScopeMono;
                })
                .AddTo(disposables);
        }

        private void DisposeNight() =>
            _nightScope.Switch(scope =>
            {
                try
                {
                    Object.Destroy(scope.gameObject);
                }
                catch (Exception exception)
                {
                    if (exception is not MissingReferenceException)
                        throw;
                }
            }, none => { });
    }
}
