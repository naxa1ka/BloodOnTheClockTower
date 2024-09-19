using System;
using System.Collections.Generic;
using BloodClockTower;
using BloodClockTower.UI;

public class Bootstrap : IDisposable
{
    private readonly List<IDisposable> _disposables = new();
    private readonly List<IPresenter> _presenters = new();
    private readonly MenuScene _menuScene;
    private readonly BoostrapContext _context;

    public Bootstrap(BoostrapContext context)
    {
        _menuScene = new MenuScene();
        _context = context;
    }

    public void Compose()
    {
        _context.CoroutineRunner.Run(_menuScene.Load());
    }

    public void Start()
    {
        new MenuPresenter(
            new MenuView(_menuScene.Context.UIDocument.ToSafetyUiDocument()),
            new MenuViewModel(
                new StartGameCommand(
                    _context.CoroutineRunner,
                    new ViewFactory<PlayerIconView>(_context.PlayerIconView)
                )
            )
        )
            .AddTo(_presenters)
            .AddTo(_disposables);

        foreach (var presenter in _presenters)
            presenter.Initialize();
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
            disposable.Dispose();
    }
}
