using System.Collections.Generic;

namespace BloodClockTower.Game
{
    public interface IVotingRound
    {
        IVotingRound DeepClone();
        IPlayerStatus Initiator { get; }
        IPlayerStatus Nominee { get; }
        IReadOnlyCollection<IPlayerStatus> Participants { get; }
        IReadOnlyCollection<IPlayerStatus> IgnoredParticipants { get; }
    }
}
