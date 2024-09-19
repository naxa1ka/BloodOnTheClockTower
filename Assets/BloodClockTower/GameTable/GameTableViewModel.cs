using System;
using System.Collections.Generic;
using BloodClockTower;
using UniRx;

public class GameTableViewModel : DisposableObject, IInitializable
{
    private readonly GameTable _model;
    private readonly ReactiveCollection<PlayerViewModel> _players;
    private readonly Dictionary<IPlayer, PlayerViewModel> _playerViewModelMapping;
    private readonly Dictionary<PlayerViewModel, IDisposable> _playerSubscriptions;
    private readonly Subject<PlayerViewModel> _clickedPlayerSubject;

    public IReadOnlyReactiveCollection<PlayerViewModel> Players => _players;
    public IObservable<PlayerViewModel> Clicked => _clickedPlayerSubject;

    public GameTableViewModel(GameTable model)
    {
        _model = model;
        _players = new ReactiveCollection<PlayerViewModel>().AddTo(disposables);
        _clickedPlayerSubject = new Subject<PlayerViewModel>().AddTo(disposables);
        _playerViewModelMapping = new Dictionary<IPlayer, PlayerViewModel>();
        _playerSubscriptions = new Dictionary<PlayerViewModel, IDisposable>();
    }

    public void Initialize()
    {
        _model.Players.ObserveAddItemWithCollection().Subscribe(AddPlayer).AddTo(disposables);
        _model.Players.ObserveRemoveItem().Subscribe(RemovePlayer).AddTo(disposables);
    }

    private void AddPlayer(IPlayer player)
    {
        var playerViewModel = new PlayerViewModel(player);
        _playerViewModelMapping[player] = playerViewModel;
        _players.Add(playerViewModel);

        var d1 = playerViewModel.Clicked.Subscribe(
            _ => _clickedPlayerSubject.OnNext(playerViewModel)
        );
        _playerSubscriptions.Add(
            playerViewModel,
            StableCompositeDisposable.Create(d1, playerViewModel)
        );
    }

    private void RemovePlayer(IPlayer player)
    {
        var playerViewModel = _playerViewModelMapping[player];
        _playerSubscriptions[playerViewModel].Dispose();
        _playerSubscriptions.Remove(playerViewModel);
        _players.Remove(playerViewModel);
    }
}
