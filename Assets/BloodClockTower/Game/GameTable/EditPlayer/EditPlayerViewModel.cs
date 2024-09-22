using System;
using Nxlk.UniRx;
using OneOf.Types;
using OneOf;
using UniRx;

namespace BloodClockTower.Game
{
    public class EditPlayerViewModel : DisposableObject, IInitializable
    {
        private readonly GameTableViewModel _gameTableViewModel;

        private readonly ReactiveProperty<bool> _isEditing;
        private readonly ReactiveProperty<OneOf<PlayerViewModel, None>> _selectedPlayer;
        
        public IReadOnlyReactiveProperty<bool> IsEditing => _isEditing;
        public IReadOnlyReactiveProperty<OneOf<PlayerViewModel, None>> SelectedPlayer => _selectedPlayer;

        public EditPlayerViewModel(GameTableViewModel gameTableViewModel)
        {
            _gameTableViewModel = gameTableViewModel;
            _isEditing = new ReactiveProperty<bool>(false).AddTo(disposables);
            _selectedPlayer = new ReactiveProperty<OneOf<PlayerViewModel, None>>(new None()).AddTo(
                disposables
            );
        }

        public void Initialize()
        {
            _gameTableViewModel.Clicked.Subscribe(SelectPlayer).AddTo(disposables);
        }

        public void StartEditing() => _isEditing.Value = true;

        public void EndEditing() => _isEditing.Value = false;

        public void ResetSelectedPlayer()
        {
            _selectedPlayer.Value.Switch(player => player.Deselect(), none => { });
            _selectedPlayer.Value = new None();
        }

        public void ChangeSelectedPlayerName(string newName)
        {
            _selectedPlayer.Value.Switch(
                model => model.ChangeName(newName),
                none => throw new ArgumentNullException()
            );
        }

        public void ReviveSelectedPlayer()
        {
            _selectedPlayer.Value.Switch(
                player => player.Revive(),
                none => throw new InvalidOperationException()
            );
        }

        public void KillSelectedPlayer()
        {
            _selectedPlayer.Value.Switch(
                player => player.Kill(),
                none => throw new InvalidOperationException()
            );
        }

        private void SelectPlayer(PlayerViewModel model)
        {
            if (!_isEditing.Value)
                return;
            _selectedPlayer.Value.Switch(player => player.Deselect(), none => { });
            _selectedPlayer.Value = model;
            _selectedPlayer.Value.Switch(player => player.Select(), none => { });
        }
    }
}