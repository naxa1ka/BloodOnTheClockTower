using UniRx;

namespace BloodClockTower.Game
{
    public interface IGame
    {
        IReadOnlyReactiveProperty<INight> CurrentNight { get; }
        void StartNewNight();
        bool IsFirstNight();
        bool IsLastNight();
        void NextNightOrStartNewNight();
        void NextNight();
        void PreviousNight();
    }
}