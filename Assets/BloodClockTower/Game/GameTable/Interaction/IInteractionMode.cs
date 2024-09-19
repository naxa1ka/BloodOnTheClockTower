namespace BloodClockTower.Game
{
    public interface IInteractionMode : IReadOnlyInteractionMode
    {
        void EnableEditName();
        void DisableEditName();
        void EnableDrag();
        void DisableDrag();
    }
}
