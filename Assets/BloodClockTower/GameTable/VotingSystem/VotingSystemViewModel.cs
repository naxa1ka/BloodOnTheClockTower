using System;
using System.Collections.Generic;
using System.Linq;
using Nxlk.Bool;
using Nxlk.LINQ;
using Nxlk.UniRx;
using UniRx;
using static BloodClockTower.PlayerViewModel;
using CollectionExtensions = Nxlk.LINQ.CollectionExtensions;

namespace BloodClockTower
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

        private PlayerViewModel Initiator =>
            _gameTableViewModel.Players.Single(
                player => player.Role.Value.HasFlag(VoteRole.Initiator)
            );

        private PlayerViewModel Nominee =>
            _gameTableViewModel.Players.Single(
                player => player.Role.Value.HasFlag(VoteRole.Nominee)
            );

        public IReadOnlyReactiveProperty<State> CurrentState => _currentState;

        public VotingSystemViewModel(
            GameTableViewModel gameTableViewModel,
            VotingHistoryViewModel votingHistoryViewModel
        )
        {
            _gameTableViewModel = gameTableViewModel;
            _votingHistoryViewModel = votingHistoryViewModel;
            _currentState = CollectionExtensions.AddTo(new ReactiveProperty<State>(State.Idle), disposables);
        }

        public void Initialize()
        {
            CollectionExtensions.AddTo(_gameTableViewModel.Clicked.Subscribe(OnPlayerClicked), disposables);
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
            _votingHistoryViewModel.Add(
                new VotingRound(
                    Initiator.Name.Value,
                    Nominee.Name.Value,
                    Participants: _gameTableViewModel
                        .Players.Where(player => player.Role.Value.HasFlag(VoteRole.Nominee))
                        .Select(playerViewModel => playerViewModel.Name.Value)
                        .ToList(),
                    IgnoredParticipants: _gameTableViewModel
                        .Players.Where(player => player.Role.Value == VoteRole.Default)
                        .Select(playerViewModel => playerViewModel.Name.Value)
                        .ToList()
                )
            );
            foreach (var player in _gameTableViewModel.Players)
                player.ClearMark();
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
