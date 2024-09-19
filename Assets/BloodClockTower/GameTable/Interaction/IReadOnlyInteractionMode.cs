using UniRx;

namespace BloodClockTower
{
    public interface IReadOnlyInteractionMode
    {
        IReadOnlyReactiveProperty<bool> CanDrag { get; }
        IReadOnlyReactiveProperty<bool> CanEditName { get; }
    }
}
