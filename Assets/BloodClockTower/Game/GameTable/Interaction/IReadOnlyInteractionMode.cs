using UniRx;

namespace BloodClockTower.Game
{
    public interface IReadOnlyInteractionMode
    {
        IReadOnlyReactiveProperty<bool> CanDrag { get; }
        IReadOnlyReactiveProperty<bool> CanEditName { get; }
    }
}
