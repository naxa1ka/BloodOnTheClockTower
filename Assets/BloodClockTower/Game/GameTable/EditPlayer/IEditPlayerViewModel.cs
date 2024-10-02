using OneOf;
using OneOf.Types;
using UniRx;

namespace BloodClockTower.Game
{
    public interface IEditPlayerViewModel
    {
        IReadOnlyReactiveProperty<bool> IsEditing { get; }
        IReadOnlyReactiveProperty<OneOf<PlayerViewModel, None>> SelectedPlayer { get; }
        void StartEditing();
        void EndEditing();
        void ResetSelectedPlayer();
        void ChangeSelectedPlayerName(string newName);
        void KillSelectedPlayer();
    }
}