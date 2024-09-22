using System.Collections.Generic;
using Nxlk.LINQ;
using Nxlk.UIToolkit;
using Nxlk.UniRx;

namespace BloodClockTower.Game
{
    public class NightScope : DisposableObject
    {
        private readonly ViewFactory<PlayerIconView> _playerIconViewFactory;
        private readonly SafetyUiDocument _safetyUiDocument;
        private readonly Game _game;
        private readonly Night _night;

        private readonly List<IInitializable> _initializables = new();
        private readonly List<IPresenter> _presenters = new();

        public NightScope(
            ViewFactory<PlayerIconView> playerIconViewFactory,
            SafetyUiDocument safetyUiDocument,
            Game game,
            Night night
        )
        {
            _playerIconViewFactory = playerIconViewFactory;
            _safetyUiDocument = safetyUiDocument;
            _game = game;
            _night = night;
        }

        public void Compose()
        {
            var gameTableViewModel = new GameTableViewModel(_night)
                .AddTo(disposables)
                .AddTo(_initializables);
            var votingHistoryViewModel = new VotingHistoryViewModel(_night)
                .AddTo(disposables)
                .AddTo(_initializables);
            var votingSystemViewModel = new VotingSystemViewModel(
                gameTableViewModel,
                votingHistoryViewModel
            )
                .AddTo(disposables)
                .AddTo(_initializables);
            var editPlayerViewModel = new EditPlayerViewModel(gameTableViewModel)
                .AddTo(_initializables)
                .AddTo(disposables);

            new GameTablePresenter(
                new GameTableView(_safetyUiDocument),
                gameTableViewModel,
                _playerIconViewFactory,
                votingHistoryViewModel
            )
                .AddTo(disposables)
                .AddTo(_presenters);

            new EditPlayerPresenter(new EditPlayerView(_safetyUiDocument), editPlayerViewModel, votingSystemViewModel)
                .AddTo(disposables)
                .AddTo(_presenters);

            new NightChangingPresenter(new NightChangingView(_safetyUiDocument), _game)
                .AddTo(disposables)
                .AddTo(_presenters);

            new VotingHistoryPresenter(
                new VotingHistoryView(_safetyUiDocument),
                votingHistoryViewModel,
                votingSystemViewModel,
                editPlayerViewModel
            )
                .AddTo(disposables)
                .AddTo(_presenters);

            new VotingSystemPresenter(
                new VotingSystemView(_safetyUiDocument),
                votingSystemViewModel,
                editPlayerViewModel
            )
                .AddTo(disposables)
                .AddTo(_presenters);
        }

        public void Start()
        {
            foreach (var initializable in _initializables)
                initializable.Initialize();
            foreach (var presenter in _presenters)
                presenter.Initialize();
        }
    }
}
