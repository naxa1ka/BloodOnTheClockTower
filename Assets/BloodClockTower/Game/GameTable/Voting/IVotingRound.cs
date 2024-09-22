using System.Collections.Generic;

namespace BloodClockTower.Game
{
    public interface IVotingRound
    {
        IVotingRound DeepClone();
        IPlayer Initiator { get; }
        IPlayer Nominee { get; }
        IReadOnlyCollection<IPlayer> Participants { get; }
        IReadOnlyCollection<IPlayer> IgnoredParticipants { get; }
    }
}
