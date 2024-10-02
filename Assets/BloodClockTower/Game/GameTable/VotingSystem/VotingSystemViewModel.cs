using System;
using System.Linq;
using Nxlk.Bool;
using Nxlk.Initialization;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class VotingSystemViewModel : DisposableObject, IInitializable, IVotingSystemViewModel
    {
        private readonly IGameTableViewModel _gameTableViewModel;
        private readonly IVotingHistoryViewModel _votingHistoryViewModel;
        private readonly ReactiveProperty<VotingSystemState> _currentState;

        private PlayerViewModel? InitiatorOrDefault =>
            _gameTableViewModel.Players.SingleOrDefault(player => player.IsInitiator);

        private PlayerViewModel Initiator =>
            InitiatorOrDefault ?? throw new NullReferenceException();

        private PlayerViewModel? NomineeOrDefault =>
            _gameTableViewModel.Players.SingleOrDefault(player => player.IsNominee);

        private PlayerViewModel Nominee => NomineeOrDefault ?? throw new NullReferenceException();
        public IReadOnlyReactiveProperty<VotingSystemState> CurrentState => _currentState;

        public VotingSystemViewModel(
            IGameTableViewModel gameTableViewModel,
            IVotingHistoryViewModel votingHistoryViewModel
        )
        {
            _gameTableViewModel = gameTableViewModel;
            _votingHistoryViewModel = votingHistoryViewModel;
            _currentState = new ReactiveProperty<VotingSystemState>(VotingSystemState.Idle).AddTo(disposables);
        }

        public void Initialize()
        {
            _gameTableViewModel.Clicked.Subscribe(OnPlayerClicked).AddTo(disposables);
        }

        private void OnPlayerClicked(PlayerViewModel player)
        {
            switch (CurrentState.Value)
            {
                case VotingSystemState.Idle:
                    return;
                case VotingSystemState.ChoosingInitiator:
                {
                    _currentState.Value = VotingSystemState.ChoosingNominee;
                    player.MarkInitiator();
                    return;
                }
                case VotingSystemState.ChoosingNominee:
                {
                    _currentState.Value = VotingSystemState.ChoosingParticipant;
                    player.MarkNominee();
                    return;
                }
                case VotingSystemState.ChoosingParticipant:
                {
                    player.IsParticipant.Switch(player.UnmarkParticipant, player.MarkParticipant);
                    return;
                }
                case VotingSystemState.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void StartVoting()
        {
            _currentState.Value = VotingSystemState.ChoosingInitiator;
        }

        public void EndVoting()
        {
            var hasEnoughParticipants = InitiatorOrDefault != null && NomineeOrDefault != null;
            hasEnoughParticipants.Switch(
                () =>
                {
                    _votingHistoryViewModel.Add(
                        new VotingRoundFromViewModelPlayers(_gameTableViewModel.Players)
                    );
                    foreach (var playerViewModel in _gameTableViewModel.Players)
                        playerViewModel.EndVoting();
                },
                () =>
                {
                    foreach (var player in _gameTableViewModel.Players)
                        player.ClearMark();
                }
            );
            _currentState.Value = VotingSystemState.Idle;
        }

        public void ResetInitiator()
        {
            Initiator.UnmarkInitiator();
            _currentState.Value = VotingSystemState.ChoosingInitiator;
        }

        public void ResetNominee()
        {
            Nominee.UnmarkNominee();
            _currentState.Value = VotingSystemState.ChoosingNominee;
        }
    }
}
