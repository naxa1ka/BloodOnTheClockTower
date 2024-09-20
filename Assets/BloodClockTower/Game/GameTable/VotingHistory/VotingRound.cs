using System.Collections.Generic;
using System.Linq;

namespace BloodClockTower.Game
{
    public interface IVotingRound
    {
        IVotingRound DeepClone();
        PlayerName Initiator { get; init; }
        PlayerName Nominee { get; init; }
        IReadOnlyCollection<PlayerName> Participants { get; init; }
        IReadOnlyCollection<PlayerName> IgnoredParticipants { get; init; }
    }

    public record VotingRound(
        PlayerName Initiator,
        PlayerName Nominee,
        IReadOnlyCollection<PlayerName> Participants,
        IReadOnlyCollection<PlayerName> IgnoredParticipants
    ) : IVotingRound
    {
        public override string ToString()
        {
            return $"{Initiator} -> {Nominee}"
                + $"\nVoted: {string.Join(", ", Participants)}"
                + $"\nIgnored: {string.Join(", ", IgnoredParticipants)}";
        }

        public IVotingRound DeepClone() =>
            new VotingRound(Initiator, Nominee, Participants, IgnoredParticipants);
    }
}
