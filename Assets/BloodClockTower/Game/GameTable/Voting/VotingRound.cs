using System.Collections.Generic;
using System.Linq;

namespace BloodClockTower.Game
{
    public record VotingRound(
        IPlayerStatus Initiator,
        IPlayerStatus Nominee,
        IReadOnlyCollection<IPlayerStatus> Participants,
        IReadOnlyCollection<IPlayerStatus> IgnoredParticipants
    ) : IVotingRound
    {
        public override string ToString()
        {
            return $"{Initiator.Name.Value} -> {Nominee.Name.Value}"
                + $"\nVoted: {string.Join(", ", Participants.Select(x => x.Name.Value))}"
                + $"\nIgnored: {string.Join(", ", IgnoredParticipants.Select(x => x.Name.Value))}";
        }

        public IVotingRound DeepClone() =>
            new VotingRound(Initiator, Nominee, Participants, IgnoredParticipants);
    }
}
