using System.Diagnostics.Contracts;
using UniRx;

namespace BloodClockTower.Game
{
    public interface IPlayerStatus
    {
        IReadOnlyReactiveProperty<PlayerName> Name { get; }
        IReadOnlyReactiveProperty<bool> IsAlive { get; }
        IReadOnlyReactiveProperty<bool> HasGhostlyVote { get; }
        IReadOnlyReactiveProperty<string> Note { get; }
        IPlayer Original { get; }

        void ChangeName(string name);
        void ChangeNote(string note);
        void Kill();
        void UseGhostlyVoice();

        [Pure]
        IPlayerStatus DeepClone();
    }
}
