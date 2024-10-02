using System;
using System.Collections.Generic;
using Nxlk.Initialization;
using Nxlk.LINQ;
using Nxlk.UniRx;
using UniRx;
using CollectionExtensions = Nxlk.LINQ.CollectionExtensions;

namespace BloodClockTower.Game
{
    public class GameTableViewModel : DisposableObject, IInitializable
    {
        private readonly Night _night;

        private readonly ReactiveCollection<PlayerViewModel> _players;
        private readonly Dictionary<IPlayerStatus, PlayerViewModel> _playerViewModelMapping;
        private readonly Dictionary<PlayerViewModel, IDisposable> _playerSubscriptions;
        private readonly Subject<PlayerViewModel> _clickedPlayerSubject;

        public IReadOnlyReactiveCollection<PlayerViewModel> Players => _players;
        public IObservable<PlayerViewModel> Clicked => _clickedPlayerSubject;

        public GameTableViewModel(Night night)
        {
            _night = night;
            _players = new ReactiveCollection<PlayerViewModel>().AddTo(disposables);
            _clickedPlayerSubject = new Subject<PlayerViewModel>().AddTo(disposables);
            _playerViewModelMapping = new Dictionary<IPlayerStatus, PlayerViewModel>();
            _playerSubscriptions = new Dictionary<PlayerViewModel, IDisposable>();
        }

        public void Initialize()
        {
            _night.Players.ObserveAddItemWithCollection().Subscribe(AddPlayer).AddTo(disposables);
            _night.Players.ObserveRemoveItem().Subscribe(RemovePlayer).AddTo(disposables);
        }

        private void AddPlayer(IPlayerStatus player)
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

        private void RemovePlayer(IPlayerStatus player)
        {
            var playerViewModel = _playerViewModelMapping[player];
            _playerSubscriptions[playerViewModel].Dispose();
            _playerSubscriptions.Remove(playerViewModel);
            _players.Remove(playerViewModel);
        }
    }
}
