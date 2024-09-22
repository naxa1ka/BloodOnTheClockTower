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
            return $"{RoundsAsString()}\n\n{IgnoredParticipantsOverallToString()}\n{ParticipantsOverallToString()}";

            string RoundsAsString() =>
                string.Join(
                    "\n\n",
                    _votingRounds.Select(
                        (round, index) => $"Round {index}" + $"\n{ToString(round)}"
                    )
                );

            string IgnoredParticipantsOverallToString() =>
                $"Overall ignored: {string.Join(", ", ToString(IgnoredParticipantsOverall))}";

            string ParticipantsOverallToString() =>
                $"Overall voted: {string.Join(", ", ToString(ParticipantsOverall))}";
        }

        private string ToString(IVotingRound round)
        {
            return $"{round.Initiator.Name.Value} -> {round.Nominee.Name.Value}"
                + $"\nVoted: {string.Join(", ", ToString(round.Participants))}";
        }

        private string ToString(IEnumerable<IPlayer> players) =>
            string.Join(", ", players.Select(x => x.Name.Value));
    }
}
