using System;
using System.Collections.Generic;

public class Bootstrap : IDisposable
{
    private readonly List<IDisposable> _disposables = new();
    private readonly List<IPresenter> _presenters = new();

    public void Compose()
    {
        var menuScene = new MenuScene();
        menuScene.Load();

        new MenuPresenter(new MenuView(menuScene.Context.MenuUIDocument.ToSafetyUiDocument()), new MenuViewModel())
            .AddTo(_presenters).AddTo(_disposables);
    }

    public void Start()
    {
        foreach (var presenter in _presenters)
            presenter.Initialize();
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
            disposable.Dispose();
    }
}