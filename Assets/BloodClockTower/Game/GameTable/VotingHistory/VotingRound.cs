using System.Collections.Generic;
using System.Linq;

namespace BloodClockTower.Game
{
    public record VotingRound(
        PlayerName Initiator,
        PlayerName Nominee,
        IReadOnlyCollection<PlayerName> Participants,
        IReadOnlyCollection<PlayerName> IgnoredParticipants
    )
    {
        public override string ToString()
        {
            return $"{Initiator.Name} -> {Nominee.Name}"
                + $"\nVoted: {string.Join(", ", Participants.Select(x => x.Name))}"
                + $"\nIgnored: {string.Join(", ", IgnoredParticipants.Select(x => x.Name))}";
        }
    }
}
