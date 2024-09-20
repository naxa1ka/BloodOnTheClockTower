using System.Diagnostics.Contracts;
using UniRx;

namespace BloodClockTower.Game
{
    public interface IPlayer
    {
        IReadOnlyReactiveProperty<PlayerName> Name { get; }
        bool IsAlive { get; }

        void ChangeName(string name);

        [Pure]
        IPlayer DeepClone();
    }
}
