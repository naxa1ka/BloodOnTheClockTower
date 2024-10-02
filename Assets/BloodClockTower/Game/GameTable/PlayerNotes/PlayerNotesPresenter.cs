using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class PlayerNotesPresenter : DisposableObject, IPresenter
    {
        private readonly IPlayerNotesView _view;
        private readonly IPlayerNotesViewModel _viewModel;
        private readonly IVotingSystemViewModel _votingSystemViewModel;
        private readonly IEditPlayerViewModel _editPlayerViewModel;

        public PlayerNotesPresenter(
            IPlayerNotesView view,
            IPlayerNotesViewModel viewModel,
            IVotingSystemViewModel votingSystemViewModel,
            IEditPlayerViewModel editPlayerViewModel
        )
        {
            _view = view;
            _viewModel = viewModel;
            _votingSystemViewModel = votingSystemViewModel;
            _editPlayerViewModel = editPlayerViewModel;
        }

        public void Initialize()
        {
            _view.OpenPlayerNotesButton.Hide();
            Observable
                .CombineLatest(
                    _votingSystemViewModel.CurrentState,
                    _editPlayerViewModel.IsEditing,
                    _editPlayerViewModel.SelectedPlayer,
                    (state, isEditingPlayer, selectedPlayer) =>
                        state == VotingSystemState.Idle
                        && isEditingPlayer
                        && selectedPlayer.IsT0
                )
                .BindToVisible(_view.OpenPlayerNotesButton)
                .AddTo(disposables);
            _viewModel.IsVisible.BindToVisible(_view.PlayerNotesContainer).AddTo(disposables);
            _view.OpenPlayerNotesButton.SubscribeOnClick(_viewModel.Show).AddTo(disposables);
            _view.ClosePlayerNotesButton.SubscribeOnClick(_viewModel.Hide).AddTo(disposables);
            EscapeObservable
                .Instance.Where(_ => _viewModel.IsVisible.Value)
                .Subscribe(_viewModel.Hide)
                .AddTo(disposables);
            _viewModel
                .IsVisible.WhereTrue()
                .Subscribe(() => _view.NoteLabel.text = _viewModel.Notes)
                .AddTo(disposables);
            _editPlayerViewModel
                .SelectedPlayer.Select(
                    playerOrNone =>
                        playerOrNone.Match(
                            player => player.Name.Value.Value,
                            none => "player is empty"
                        )
                )
                .Subscribe(text => _view.PlayerHeaderLabel.Text = text)
                .AddTo(disposables);
            _view.NoteInputField.ObserveText().Subscribe(_viewModel.ChangeNote).AddTo(disposables);
            _viewModel
                .SelectedPlayerNote.Select(
                    noteOrNone => noteOrNone.Match(note => note, none => "player is empty")
                )
                .Subscribe(_view.NoteInputField.SetValueWithoutNotify)
                .AddTo(disposables);
        }
    }
}
