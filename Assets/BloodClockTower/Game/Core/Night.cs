using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UniRx;

namespace BloodClockTower.Game
{
    public class Night
    {
        private readonly ReactiveCollection<IPlayer> _players;
        private readonly ReactiveCollection<IVotingRound> _votingRounds;
        private readonly ReactiveCollection<INote> _notes;
        public int Number { get; }

        public IReadOnlyReactiveCollection<IVotingRound> VotingRounds => _votingRounds;
        public IReadOnlyReactiveCollection<INote> Notes => _notes;
        public IReadOnlyReactiveCollection<IPlayer> Players => _players;

        public Night()
            : this(Enumerable.Empty<IPlayer>()) { }

        public Night(IEnumerable<IPlayer> players)
            : this(1, players, Enumerable.Empty<IVotingRound>(), Enumerable.Empty<INote>()) { }

        public Night(
            int number,
            IEnumerable<IPlayer> players,
            IEnumerable<IVotingRound> votingRounds,
            IEnumerable<INote> notes
        )
        {
            Number = number;
            _players = new ReactiveCollection<IPlayer>(players);
            _votingRounds = new ReactiveCollection<IVotingRound>(votingRounds);
            _notes = new ReactiveCollection<INote>(notes);
        }

        [Pure]
        public Night NextNight()
        {
            return new Night(
                Number + 1,
                _players.Select(x => x.DeepClone()),
                Enumerable.Empty<IVotingRound>(),
                Enumerable.Empty<INote>()
            );
        }

        public void Add(IVotingRound votingRound) => _votingRounds.Add(votingRound);
    }
}
