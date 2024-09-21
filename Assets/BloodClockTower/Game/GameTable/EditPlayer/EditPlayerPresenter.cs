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

        private IDisposable _selectedPlayerSubscription = Disposable.Empty;
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
            Observable
                .CombineLatest(
                    _isEditing,
                    _selectedPlayer,
                    (isEditing, selectedPlayer) => isEditing && selectedPlayer.IsT0
                )
                .BindToVisible(_view.NameInputField)
                .AddTo(disposables);

            Observable
                .CombineLatest(
                    _isEditing,
                    _selectedPlayer,
                    (isEditing, selectedPlayer) => (isSelected: isEditing, selectedPlayer)
                )
                .Subscribe(tuple => SubscribeOnKillButtons(tuple.isSelected, tuple.selectedPlayer))
                .AddTo(disposables);

            _selectedPlayer
                .Subscribe(
                    selectedPlayer =>
                        _view.NameInputField.SetValueWithoutNotify(
                            selectedPlayer.Match(player => player.Name.Value.Value, none => "None")
                        )
                )
                .AddTo(disposables);
            _view
                .KillPlayerButton.SubscribeOnClick(
                    () =>
                        _selectedPlayer.Value.Switch(
                            player => player.Kill(),
                            none => throw new InvalidOperationException()
                        )
                )
                .AddTo(disposables);
            _view
                .RevivePlayerButton.SubscribeOnClick(
                    () =>
                        _selectedPlayer.Value.Switch(
                            player => player.Revive(),
                            none => throw new InvalidOperationException()
                        )
                )
                .AddTo(disposables);
        }

        private void SubscribeOnKillButtons(bool isEditing, OneOf<PlayerViewModel, None> player)
        {
            _selectedPlayerSubscription.Dispose();
            if (!isEditing)
            {
                HideButtons();
                return;
            }
            if (!player.TryPickT0(out var selectedPlayer, out _))
            {
                HideButtons();
                return;
            }
            selectedPlayer.IsAlive.BindToVisible(_view.KillPlayerButton).AddTo(disposables);
            selectedPlayer
                .IsAlive.InverseBool()
                .BindToVisible(_view.RevivePlayerButton)
                .AddTo(disposables);

            return;
            void HideButtons()
            {
                _view.KillPlayerButton.Hide();
                _view.RevivePlayerButton.Hide();
            }
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
