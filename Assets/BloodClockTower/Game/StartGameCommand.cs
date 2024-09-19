using Nxlk.Coroutine;
using Nxlk.UIToolkit;

namespace BloodClockTower.Game
{
    public class StartGameCommand
    {
        private readonly CoroutineRunner _coroutineRunner;
        private readonly ViewFactory<PlayerIconView> _playerIconViewFactory;

        public StartGameCommand(
            CoroutineRunner coroutineRunner,
            ViewFactory<PlayerIconView> playerIconViewFactory
        )
        {
            _coroutineRunner = coroutineRunner;
            _playerIconViewFactory = playerIconViewFactory;
        }

        public void Execute(int playersAmount)
        {
            var gameScene = new GameScene();
            _coroutineRunner.Run(
                gameScene.Load(),
                () =>
                {
                    var safetyUiDocument = gameScene.Context.UIDocument.ToSafetyUiDocument();

                    var interactionMode = new InteractionMode();
                    var gameTable = new GameTable(playersAmount);
                    var gameTableViewModel = new GameTableViewModel(gameTable);
                    var votingHistoryViewModel = new VotingHistoryViewModel();
                    var votingSystemViewModel = new VotingSystemViewModel(
                        gameTableViewModel,
                        votingHistoryViewModel
                    );

                    var interactionModeView = new InteractionModeView(safetyUiDocument);
                    var interactionModePresenter = new InteractionModePresenter(
                        interactionModeView,
                        interactionMode
                    );

                    var gameTableView = new GameTableView(safetyUiDocument);
                    var gameTablePresenter = new GameTablePresenter(
                        gameTableView,
                        gameTableViewModel,
                        _playerIconViewFactory,
                        interactionMode,
                        votingHistoryViewModel
                    );

                    var votingHistoryView = new VotingHistoryView(safetyUiDocument);
                    var votingHistoryPresenter = new VotingHistoryPresenter(
                        votingHistoryView,
                        votingHistoryViewModel,
                        votingSystemViewModel
                    );

                    var votingSystemView = new VotingSystemView(safetyUiDocument);
                    var votingSystemPresenter = new VotingSystemPresenter(
                        votingSystemView,
                        votingSystemViewModel,
                        gameTableViewModel
                    );

                    gameTableViewModel.Initialize();
                    votingSystemViewModel.Initialize();
                    votingHistoryViewModel.Initialize();

                    interactionModePresenter.Initialize();
                    gameTablePresenter.Initialize();
                    votingSystemPresenter.Initialize();
                    votingHistoryPresenter.Initialize();
                }
            );
        }
    }
}
