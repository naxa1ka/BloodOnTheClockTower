using System;
using System.Linq;
using Nxlk.Bool;
using Nxlk.LINQ;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using OneOf;
using OneOf.Types;
using UniRx;

namespace BloodClockTower.Game
{
    public class EditPlayerPresenter : DisposableObject, IPresenter
    {
        private readonly EditPlayerView _view;
        private readonly GameTableViewModel _gameTableViewModel;

        private readonly ReactiveProperty<bool> _isEditing;
        private readonly ReactiveProperty<OneOf<PlayerViewModel, None>> _selectedPlayer;

        public EditPlayerPresenter(EditPlayerView view, GameTableViewModel gameTableViewModel)
        {
            _view = view;
            _gameTableViewModel = gameTableViewModel;
            _isEditing = new ReactiveProperty<bool>(false).AddTo(disposables);
            _selectedPlayer = new ReactiveProperty<OneOf<PlayerViewModel, None>>(new None()).AddTo(
                disposables
            );
        }

        public void Initialize()
        {
            _gameTableViewModel.Clicked.Subscribe(SelectPlayer).AddTo(disposables);
            _view.EndEditingButton.SubscribeOnClick(ResetSelectedPlayer).AddTo(disposables);
            _view
                .StartEditingButton.SubscribeOnClick(() => _isEditing.Value = true)
                .AddTo(disposables);
            _view
                .EndEditingButton.SubscribeOnClick(() => _isEditing.Value = false)
                .AddTo(disposables);
            _view
                .NameInputField.ObserveText()
                .Subscribe(OnNameInputFieldChanged)
                .AddTo(disposables);
            _isEditing.BindToVisible(_view.EndEditingButton).AddTo(disposables);
            _isEditing.InverseBool().BindToVisible(_view.StartEditingButton).AddTo(disposables);
            _isEditing
                .CombineLatest(
                    _selectedPlayer,
                    (isSelected, selectedPlayer) => isSelected && selectedPlayer.IsT0
                )
                .BindToVisible(_view.NameInputField)
                .AddTo(disposables);
            _selectedPlayer
                .Subscribe(
                    selectedPlayer =>
                        _view.NameInputField.SetValueWithoutNotify(
                            selectedPlayer.Match(player => player.Name.Value.Value, none => "None")
                        )
                )
                .AddTo(disposables);
        }

        private void ResetSelectedPlayer()
        {
            _selectedPlayer.Value.Switch(player => player.Deselect(), none => { });
            _selectedPlayer.Value = new None();
        }

        private void OnNameInputFieldChanged(string newName)
        {
            _selectedPlayer.Value.Switch(
                model => model.ChangeName(newName),
                none => throw new ArgumentNullException()
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
