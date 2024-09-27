using UniRx;

namespace BloodClockTower.Game
{
    public interface IPlayer
    {
        IReadOnlyReactiveProperty<PlayerName> Name { get; }
        void ChangeName(string name);
    }
}