using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BloodClockTower.Game
{
    public class VotingHistoryPresenter : DisposableObject, IPresenter
    {
        private readonly VotingHistoryView _view;
        private readonly VotingHistoryViewModel _viewModel;
        private readonly VotingSystemViewModel _votingSystemViewModel;
        private readonly EditPlayerViewModel _editPlayerViewModel;
        private readonly UIToolkitEventSystem _uiToolkitEventSystem;

        public VotingHistoryPresenter(
            VotingHistoryView view,
            VotingHistoryViewModel viewModel,
            VotingSystemViewModel votingSystemViewModel,
            EditPlayerViewModel editPlayerViewModel,
            UIToolkitEventSystem uiToolkitEventSystem
        )
        {
            _view = view;
            _viewModel = viewModel;
            _votingSystemViewModel = votingSystemViewModel;
            _editPlayerViewModel = editPlayerViewModel;
            _uiToolkitEventSystem = uiToolkitEventSystem;
        }

        public void Initialize()
        {
            _view.OpenVotingHistoryButton.Hide();
            Observable
                .CombineLatest(
                    _viewModel.VotingRounds.ObserveCountChangedWithCount(),
                    _votingSystemViewModel.CurrentState,
                    _editPlayerViewModel.IsEditing,
                    (votingRoundsCount, state, isEditingPlayer) =>
                        votingRoundsCount > 0
                        && state == VotingSystemViewModel.State.Idle
                        && !isEditingPlayer
                )
                .BindToVisible(_view.OpenVotingHistoryButton)
                .AddTo(disposables);
            _viewModel.IsVisible.BindToVisible(_view.VotingHistoryContainer).AddTo(disposables);
            _view.OpenVotingHistoryButton.SubscribeOnClick(_viewModel.Show).AddTo(disposables);
            _view.CloseVotingHistoryButton.SubscribeOnClick(_viewModel.Hide).AddTo(disposables);
            new EscapeObservable()
                .Where(_ => _viewModel.IsVisible.Value)
                .Subscribe(_viewModel.Hide)
                .AddTo(disposables);
            _viewModel
                .IsVisible.WhereTrue()
                .Subscribe(() => _view.VotingHistoryLabel.text = _viewModel.VotingRounds.ToString())
                .AddTo(disposables);
            _view
                .EditNoteVotingHistoryButton.SubscribeOnClick(() =>
                {
                    _viewModel.StartEditingNote();
                    _uiToolkitEventSystem.SelectEventSystem();
                    _view.NoteInputField.Focus();
                })
                .AddTo(disposables);
            _view
                .DoneNoteVotingHistoryButton.SubscribeOnClick(_viewModel.EndEditingNote)
                .AddTo(disposables);
            _viewModel
                .IsEditingNote.InverseBool()
                .BindToVisible(_view.EditNoteVotingHistoryButton)
                .AddTo(disposables);
            _viewModel
                .IsEditingNote.InverseBool()
                .BindToVisible(_view.NoteLabel)
                .AddTo(disposables);
            _viewModel
                .IsEditingNote.BindToVisible(_view.DoneNoteVotingHistoryButton)
                .AddTo(disposables);
            _viewModel.IsEditingNote.BindToVisible(_view.NoteInputField).AddTo(disposables);
            _view.NoteInputField.ObserveText().Subscribe(_viewModel.ChangeNote).AddTo(disposables);
            _viewModel
                .VotingRounds.Note.Subscribe(_view.NoteInputField.SetValueWithoutNotify)
                .AddTo(disposables);
            _viewModel
                .VotingRounds.Note.Subscribe(note => _view.NoteLabel.text = note)
                .AddTo(disposables);
        }
    }
}
