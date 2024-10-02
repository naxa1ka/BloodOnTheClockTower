using System;
using System.Linq;
using Nxlk.Bool;
using Nxlk.Initialization;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class VotingSystemViewModel : DisposableObject, IInitializable
    {
        private readonly GameTableViewModel _gameTableViewModel;
        private readonly VotingHistoryViewModel _votingHistoryViewModel;

        public enum State
        {
            None,
            Idle,
            ChoosingInitiator,
            ChoosingNominee,
            ChoosingParticipant,
        }

        private readonly ReactiveProperty<State> _currentState;

        private PlayerViewModel? InitiatorOrDefault =>
            _gameTableViewModel.Players.SingleOrDefault(player => player.IsInitiator);

        private PlayerViewModel Initiator =>
            InitiatorOrDefault ?? throw new NullReferenceException();

        private PlayerViewModel? NomineeOrDefault =>
            _gameTableViewModel.Players.SingleOrDefault(player => player.IsNominee);

        private PlayerViewModel Nominee => NomineeOrDefault ?? throw new NullReferenceException();
        public IReadOnlyReactiveProperty<State> CurrentState => _currentState;

        public VotingSystemViewModel(
            GameTableViewModel gameTableViewModel,
            VotingHistoryViewModel votingHistoryViewModel
        )
        {
            _gameTableViewModel = gameTableViewModel;
            _votingHistoryViewModel = votingHistoryViewModel;
            _currentState = new ReactiveProperty<State>(State.Idle).AddTo(disposables);
        }

        public void Initialize()
        {
            _gameTableViewModel.Clicked.Subscribe(OnPlayerClicked).AddTo(disposables);
        }

        private void OnPlayerClicked(PlayerViewModel player)
        {
            switch (CurrentState.Value)
            {
                case State.Idle:
                    return;
                case State.ChoosingInitiator:
                {
                    _currentState.Value = State.ChoosingNominee;
                    player.MarkInitiator();
                    return;
                }
                case State.ChoosingNominee:
                {
                    _currentState.Value = State.ChoosingParticipant;
                    player.MarkNominee();
                    return;
                }
                case State.ChoosingParticipant:
                {
                    player.IsParticipant.Switch(player.UnmarkParticipant, player.MarkParticipant);
                    return;
                }
                case State.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void StartVoting()
        {
            _currentState.Value = State.ChoosingInitiator;
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
            _currentState.Value = State.Idle;
        }

        public void ResetInitiator()
        {
            Initiator.UnmarkInitiator();
            _currentState.Value = State.ChoosingInitiator;
        }

        public void ResetNominee()
        {
            Nominee.UnmarkNominee();
            _currentState.Value = State.ChoosingNominee;
        }
    }
}
