using UniRx;

namespace BloodClockTower.Game
{
    public interface ISwapPlayersViewModel
    {
        IReadOnlyReactiveProperty<bool> IsEditing { get; }
        void StartEditing();
        void EndEditing();
    }
}