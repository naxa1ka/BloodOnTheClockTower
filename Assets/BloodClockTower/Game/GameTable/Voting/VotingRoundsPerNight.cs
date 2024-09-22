using System;
using System.Collections.Generic;
using System.Linq;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class VotingRoundsPerNight : DisposableObject
    {
        private const string EmptyNote = "Empty note";
        
        private readonly ReactiveProperty<string> _note;
        private readonly ReactiveCollection<IVotingRound> _votingRounds;

        private IEnumerable<IPlayer> ParticipantsOverall => _votingRounds
            .SelectMany(round => round.Participants)
            .Distinct();
        private IEnumerable<IPlayer> IgnoredParticipantsOverall => _votingRounds
            .SelectMany(round => round.IgnoredParticipants)
            .Except(ParticipantsOverall)
            .Distinct();
        
        public IReadOnlyReactiveProperty<string> Note => _note;

        public static VotingRoundsPerNight Empty => new(Enumerable.Empty<IVotingRound>());

        public VotingRoundsPerNight(IEnumerable<IVotingRound> votingRounds)
        {
            _note = new ReactiveProperty<string>(EmptyNote).AddTo(disposables);
            _votingRounds = new ReactiveCollection<IVotingRound>(votingRounds).AddTo(disposables);
        }

        public IObservable<int> ObserveCountChangedWithCount() => _votingRounds.ObserveCountChangedWithCount();

        public void Add(IVotingRound votingRound) => _votingRounds.Add(votingRound);

        public void SetDefaultNoteIfItIsEmpty()
        {
            if (string.IsNullOrEmpty(_note.Value)) 
                _note.Value = EmptyNote;
        }
        
        public void ChangeNote(string note) => _note.Value = note;
        
        public override string ToString()
        {
            return $"{RoundsAsString()}\n\n{IgnoredParticipantsOverallToString()}{ParticipantsOverallToString()}";

            string RoundsAsString() =>
                string.Join(
                    "\n\n",
                    _votingRounds.Select(
                        (round, index) => $"<b>Round {index}</b>" + $"\n{ToString(round)}"
                    )
                );

            string IgnoredParticipantsOverallToString()
            {
                if (!IgnoredParticipantsOverall.Any())
                    return string.Empty;
                return $"<b>Overall ignored</b>: {string.Join(", ", ToString(IgnoredParticipantsOverall))}\n";
            }

            string ParticipantsOverallToString() =>
                $"<b>Overall voted</b>: {string.Join(", ", ToString(ParticipantsOverall))}";
        }

        private string ToString(IVotingRound round)
        {
            return $"{round.Initiator.Name.Value} -> {round.Nominee.Name.Value}"
                + $"\n<b>Voted</b>: {string.Join(", ", ToString(round.Participants))}";
        }

        private string ToString(IEnumerable<IPlayer> players) =>
            string.Join(", ", players.Select(x => x.Name.Value));
    }
}
