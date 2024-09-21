using System.Diagnostics.Contracts;
using UniRx;

namespace BloodClockTower.Game
{
    public interface IPlayer
    {
        IReadOnlyReactiveProperty<PlayerName> Name { get; }
        IReadOnlyReactiveProperty<bool> IsAlive { get; }

        void ChangeName(string name);
        void Kill();
        void Revive();

        [Pure]
        IPlayer DeepClone();
    }
}
