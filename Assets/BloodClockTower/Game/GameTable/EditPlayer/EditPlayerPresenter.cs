using System;
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
        private readonly IEditPlayerView _view;
        private readonly IEditPlayerViewModel _viewModel;
        private readonly IVotingSystemViewModel _votingSystemViewModel;
        private IDisposable _selectedPlayerSubscription = Disposable.Empty;

        public EditPlayerPresenter(
            IEditPlayerView view,
            IEditPlayerViewModel viewModel,
            IVotingSystemViewModel votingSystemViewModel
        )
        {
            _view = view;
            _viewModel = viewModel;
            _votingSystemViewModel = votingSystemViewModel;
        }

        public void Initialize()
        {
            _view
                .EndEditingButton.SubscribeOnClick(_viewModel.ResetSelectedPlayer)
                .AddTo(disposables);
            _view.StartEditingButton.SubscribeOnClick(_viewModel.StartEditing).AddTo(disposables);
            _view.EndEditingButton.SubscribeOnClick(_viewModel.EndEditing).AddTo(disposables);
            var isVotingAsObservable = _votingSystemViewModel.CurrentState.Select(
                state => state != VotingSystemState.Idle
            );
            _view
                .NameInputField.ObserveText()
                .Subscribe(_viewModel.ChangeSelectedPlayerName)
                .AddTo(disposables);
            Observable
                .CombineLatest(
                    _viewModel.IsEditing,
                    isVotingAsObservable,
                    (isEditing, isVoting) => isEditing && !isVoting
                )
                .BindToVisible(_view.EndEditingButton)
                .AddTo(disposables);
            Observable
                .CombineLatest(
                    _viewModel.IsEditing.InverseBool(),
                    isVotingAsObservable,
                    (isEditing, isVoting) => isEditing && !isVoting
                )
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
            _selectedPlayerSubscription = selectedPlayer.IsAlive.BindToVisible(_view.KillPlayerButton);

            return;
            void HideButtons()
            {
                _view.KillPlayerButton.Hide();
            }
        }
    }
}
