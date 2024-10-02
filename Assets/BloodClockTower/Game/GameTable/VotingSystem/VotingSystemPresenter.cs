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
        private readonly IVotingSystemView _view;
        private readonly IVotingSystemViewModel _model;
        private readonly IEditPlayerViewModel _editPlayerViewModel;

        public VotingSystemPresenter(
            IVotingSystemView view,
            IVotingSystemViewModel model,
            IEditPlayerViewModel editPlayerViewModel
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

        private void Update(VotingSystemState votingSystemState, bool isPlayerEditing)
        {
            UpdateButtonsVisibility(votingSystemState, isPlayerEditing);
            UpdateLabelVisibility(votingSystemState, isPlayerEditing);
            UpdateLabel(votingSystemState);
        }

        private void UpdateButtonsVisibility(VotingSystemState votingSystemState, bool isPlayerEditing)
        {
            _view.EndVotingButton.SetVisible(!isPlayerEditing && votingSystemState != VotingSystemState.Idle);
            _view.StartVotingButton.SetVisible(!isPlayerEditing && votingSystemState == VotingSystemState.Idle);
            _view.ResetInitiatorButton.SetVisible(
                !isPlayerEditing && votingSystemState == VotingSystemState.ChoosingNominee
            );
            _view.ResetNomineeButton.SetVisible(
                !isPlayerEditing && votingSystemState == VotingSystemState.ChoosingParticipant
            );
        }

        private void UpdateLabelVisibility(VotingSystemState votingSystemState, bool isPlayerEditing) =>
            _view.StateLabelContainer.SetVisible(!isPlayerEditing && votingSystemState != VotingSystemState.Idle);

        private void UpdateLabel(VotingSystemState votingSystemState)
        {
            var stateLabelText = votingSystemState switch
            {
                VotingSystemState.Idle => "Idle",
                VotingSystemState.ChoosingInitiator => "Choose initiator",
                VotingSystemState.ChoosingNominee => "Choose nominee",
                VotingSystemState.ChoosingParticipant => "Choose participant",
                _ => throw new ArgumentOutOfRangeException(nameof(votingSystemState), votingSystemState, null)
            };
            _view.StateLabel.text = stateLabelText;
        }
    }
}
