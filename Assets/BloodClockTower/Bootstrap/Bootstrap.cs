using System;
using System.Collections.Generic;
using BloodClockTower.Game;
using BloodClockTower.Menu;
using Cysharp.Threading.Tasks;
using Nxlk.LINQ;
using Nxlk.UIToolkit;
using OneOf;
using OneOf.Types;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BloodClockTower.Bootstrap
{
    public class Bootstrap : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();
        private readonly List<IPresenter> _presenters = new();
        private readonly MenuScene _menuScene;
        private readonly BoostrapContext _context;
        private OneOf<GameScopeMono, None> _gameScope = new None();

        public Bootstrap(BoostrapContext context)
        {
            _menuScene = new MenuScene();
            _context = context;
        }

        public async UniTask Load()
        {
            await _menuScene.Load();
        }

        public void Compose()
        {
            new MenuPresenter(
                new MenuView(_menuScene.Context.UIDocument.ToSafetyUiDocument()),
                new StartGameCommand(
                    new ViewFactory<PlayerIconView>(_context.PlayerIconView),
                    scope => _gameScope = scope
                )
            )
                .AddTo(_presenters)
                .AddTo(_disposables);
        }

        public void Initialize()
        {
            foreach (var presenter in _presenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
            _gameScope.Switch(
                scope =>
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
                },
                none => { }
            );
        }
    }
}
