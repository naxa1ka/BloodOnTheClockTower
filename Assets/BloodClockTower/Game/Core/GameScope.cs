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
        private OneOf<NightScopeMono, None> _nightScope = new None();

        public GameScope(
            ViewFactory<PlayerIconView> playerIconViewFactory,
            SafetyUiDocument safetyUiDocument,
            int playersAmount
        )
        {
            _safetyUiDocument = safetyUiDocument;
            _playerIconViewFactory = playerIconViewFactory;
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
                        night
                    );

                    _nightScope = nightScopeMono;
                })
                .AddTo(disposables);
        }

        private void DisposeNight() =>
            _nightScope.Switch(scope => Object.Destroy(scope.gameObject), none => { });
    }
}
