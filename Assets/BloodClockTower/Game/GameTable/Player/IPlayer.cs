using UniRx;

namespace BloodClockTower.Game
{
    public interface IPlayer
    {
        IReadOnlyReactiveProperty<PlayerName> Name { get; }
        bool IsAlive { get; }
        void Kill();
        void SetName(string name);
    }
}
