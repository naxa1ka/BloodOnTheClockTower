using OneOf;
using OneOf.Types;
using UniRx;

namespace BloodClockTower.Game
{
    public interface IPlayerNotesViewModel
    {
        IReadOnlyReactiveProperty<bool> IsVisible { get; }
        string Notes { get; }
        IReadOnlyReactiveProperty<OneOf<string, None>> SelectedPlayerNote { get; }
        void Initialize();
        void ChangeNote(string note);
        void Show();
        void Hide();
    }
}