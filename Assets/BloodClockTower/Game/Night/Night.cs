using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public interface INight : IDisposable
    {
        int Number { get; }
        VotingRoundsPerNight VotingRounds { get; }
        IReadOnlyReactiveCollection<INote> Notes { get; }
        IReadOnlyReactiveCollection<IPlayerStatus> Players { get; }
        INight NextNight();
    }

    public class Night : DisposableObject, INight
    {
        private readonly ReactiveCollection<IPlayerStatus> _players;
        private readonly ReactiveCollection<INote> _notes;

        public int Number { get; }
        public VotingRoundsPerNight VotingRounds { get; }
        public IReadOnlyReactiveCollection<INote> Notes => _notes;
        public IReadOnlyReactiveCollection<IPlayerStatus> Players => _players;

        public Night(IEnumerable<IPlayerStatus> players)
            : this(1, players, VotingRoundsPerNight.Empty, Enumerable.Empty<INote>()) { }

        private Night(
            int number,
            IEnumerable<IPlayerStatus> players,
            VotingRoundsPerNight votingRounds,
            IEnumerable<INote> notes
        )
        {
            Number = number;
            VotingRounds = votingRounds.AddTo(disposables);
            _players = new ReactiveCollection<IPlayerStatus>(players).AddTo(disposables);
            _notes = new ReactiveCollection<INote>(notes).AddTo(disposables);
        }

        [Pure]
        public INight NextNight()
        {
            return new Night(
                Number + 1,
                _players.Select(playerStatus => playerStatus.DeepClone()),
                VotingRoundsPerNight.Empty,
                Enumerable.Empty<INote>()
            );
        }
    }
}
