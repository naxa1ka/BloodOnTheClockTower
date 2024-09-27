using System.Diagnostics.Contracts;
using UniRx;

namespace BloodClockTower.Game
{
    public interface IPlayerStatus
    {
        IReadOnlyReactiveProperty<PlayerName> Name { get; }
        IReadOnlyReactiveProperty<bool> IsAlive { get; }
        IReadOnlyReactiveProperty<bool> HasGhostlyVote { get; }

        void ChangeName(string name);
        void Kill();
        void UseGhostlyVoice();

        [Pure]
        IPlayerStatus DeepClone();
    }
}
