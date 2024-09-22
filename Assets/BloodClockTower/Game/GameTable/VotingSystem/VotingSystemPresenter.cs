using System;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using static BloodClockTower.Game.VotingSystemViewModel;

namespace BloodClockTower.Game
{
    public class VotingSystemPresenter : DisposableObject, IPresenter
    {
        private readonly VotingSystemView _view;
        private readonly VotingSystemViewModel _model;
        private readonly EditPlayerViewModel _editPlayerViewModel;

        public VotingSystemPresenter(
            VotingSystemView view,
            VotingSystemViewModel model,
            EditPlayerViewModel editPlayerViewModel
        )
        {
            _view = view;
            _model = model;
            _editPlayerViewModel = editPlayerViewModel;
        }

        public void Initialize()
        {
            _view.StartVotingButton.SubscribeOnClick(_model.StartVoting).AddTo(disposables);
            _view.EndVotingButton.SubscribeOnClick(_model.EndVoting).AddTo(disposables);
            _view.ResetInitiatorButton.SubscribeOnClick(_model.ResetInitiator).AddTo(disposables);
            _view.ResetNomineeButton.SubscribeOnClick(_model.ResetNominee).AddTo(disposables);
            _model
                .CurrentState.CombineLatest(
                    _editPlayerViewModel.IsEditing,
                    (state, isPlayerEditing) => (state, isPlayerEditing)
                )
                .Subscribe(tuple => Update(tuple.state, tuple.isPlayerEditing))
                .AddTo(disposables);
        }

        private void Update(State state, bool isPlayerEditing)
        {
            UpdateButtonsVisibility(state, isPlayerEditing);
            UpdateLabelVisibility(state, isPlayerEditing);
            UpdateLabel(state);
        }

        private void UpdateButtonsVisibility(State state, bool isPlayerEditing)
        {
            _view.EndVotingButton.SetVisible(!isPlayerEditing && state != State.Idle);
            _view.StartVotingButton.SetVisible(!isPlayerEditing && state == State.Idle);
            _view.ResetInitiatorButton.SetVisible(
                !isPlayerEditing && state == State.ChoosingNominee
            );
            _view.ResetNomineeButton.SetVisible(
                !isPlayerEditing && state == State.ChoosingParticipant
            );
        }

        private void UpdateLabelVisibility(State state, bool isPlayerEditing) =>
            _view.StateLabelContainer.SetVisible(!isPlayerEditing && state != State.Idle);

        private void UpdateLabel(State state)
        {
            var stateLabelText = state switch
            {
                State.Idle => "Idle",
                State.ChoosingInitiator => "Choose initiator",
                State.ChoosingNominee => "Choose nominee",
                State.ChoosingParticipant => "Choose participant",
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
            _view.StateLabel.text = stateLabelText;
        }
    }
}
