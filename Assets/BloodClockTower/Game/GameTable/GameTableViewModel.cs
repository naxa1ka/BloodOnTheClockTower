using System;
using System.Collections.Generic;
using Nxlk.UniRx;
using UniRx;
using CollectionExtensions = Nxlk.LINQ.CollectionExtensions;

namespace BloodClockTower.Game
{
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
            _players = CollectionExtensions.AddTo(new ReactiveCollection<PlayerViewModel>(), disposables);
            _clickedPlayerSubject = CollectionExtensions.AddTo(new Subject<PlayerViewModel>(), disposables);
            _playerViewModelMapping = new Dictionary<IPlayer, PlayerViewModel>();
            _playerSubscriptions = new Dictionary<PlayerViewModel, IDisposable>();
        }

        public void Initialize()
        {
            CollectionExtensions.AddTo(_model.Players.ObserveAddItemWithCollection().Subscribe(AddPlayer), disposables);
            CollectionExtensions.AddTo(_model.Players.ObserveRemoveItem().Subscribe(RemovePlayer), disposables);
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
}
