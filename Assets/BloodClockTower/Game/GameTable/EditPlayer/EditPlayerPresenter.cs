﻿using System;
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
        private readonly EditPlayerViewModel _viewModel;
        private IDisposable _selectedPlayerSubscription = Disposable.Empty;

        public EditPlayerPresenter(EditPlayerView view, EditPlayerViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            _view
                .EndEditingButton.SubscribeOnClick(_viewModel.ResetSelectedPlayer)
                .AddTo(disposables);
            _view.StartEditingButton.SubscribeOnClick(_viewModel.StartEditing).AddTo(disposables);
            _view.EndEditingButton.SubscribeOnClick(_viewModel.EndEditing).AddTo(disposables);
            _view
                .NameInputField.ObserveText()
                .Subscribe(_viewModel.ChangeSelectedPlayerName)
                .AddTo(disposables);
            _viewModel.IsEditing.BindToVisible(_view.EndEditingButton).AddTo(disposables);
            _viewModel
                .IsEditing.InverseBool()
                .BindToVisible(_view.StartEditingButton)
                .AddTo(disposables);
            Observable
                .CombineLatest(
                    _viewModel.IsEditing,
                    _viewModel.SelectedPlayer,
                    (isEditing, selectedPlayer) => isEditing && selectedPlayer.IsT0
                )
                .BindToVisible(_view.NameInputField)
                .AddTo(disposables);

            Observable
                .CombineLatest(
                    _viewModel.IsEditing,
                    _viewModel.SelectedPlayer,
                    (isEditing, selectedPlayer) => (isSelected: isEditing, selectedPlayer)
                )
                .Subscribe(tuple => SubscribeOnKillButtons(tuple.isSelected, tuple.selectedPlayer))
                .AddTo(disposables);

            _viewModel
                .SelectedPlayer.Subscribe(
                    selectedPlayer =>
                        _view.NameInputField.SetValueWithoutNotify(
                            selectedPlayer.Match(player => player.Name.Value.Value, none => "None")
                        )
                )
                .AddTo(disposables);
            _view
                .KillPlayerButton.SubscribeOnClick(_viewModel.KillSelectedPlayer)
                .AddTo(disposables);
            _view
                .RevivePlayerButton.SubscribeOnClick(_viewModel.ReviveSelectedPlayer)
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
            var d1 = selectedPlayer.IsAlive.BindToVisible(_view.KillPlayerButton);
            var d2 = selectedPlayer.IsAlive.InverseBool().BindToVisible(_view.RevivePlayerButton);
            _selectedPlayerSubscription = StableCompositeDisposable.Create(d1, d2);

            return;
            void HideButtons()
            {
                _view.KillPlayerButton.Hide();
                _view.RevivePlayerButton.Hide();
            }
        }
    }
}
