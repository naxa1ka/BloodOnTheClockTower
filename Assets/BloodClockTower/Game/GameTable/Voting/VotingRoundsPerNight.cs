using System.Collections.Generic;
using System.Linq;

namespace BloodClockTower.Game
{
    public class VotingRoundsPerNight
    {
        private readonly IReadOnlyCollection<IVotingRound> _votingRounds;
        public IReadOnlyCollection<IPlayer> ParticipantsOverall { get; }
        public IReadOnlyCollection<IPlayer> IgnoredParticipantsOverall { get; }

        public VotingRoundsPerNight(IEnumerable<IVotingRound> votingRounds)
        {
            _votingRounds = votingRounds.ToList();
            ParticipantsOverall = _votingRounds
                .SelectMany(round => round.Participants)
                .Distinct()
                .ToList();
            IgnoredParticipantsOverall = _votingRounds
                .SelectMany(round => round.IgnoredParticipants)
                .Except(ParticipantsOverall)
                .Distinct()
                .ToList();
        }

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
                if (IgnoredParticipantsOverall.Count == 0)
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
