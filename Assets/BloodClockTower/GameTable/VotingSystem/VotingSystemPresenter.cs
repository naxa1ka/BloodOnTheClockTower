using System;
using System.Diagnostics;
using BloodClockTower.UI;
using UniRx;
using static BloodClockTower.VotingSystemViewModel;

namespace BloodClockTower
{
    public class VotingSystemPresenter : DisposableObject, IPresenter
    {
        private readonly VotingSystemView _view;
        private readonly VotingSystemViewModel _model;
        private readonly GameTableViewModel _gameTableViewModel;

        public VotingSystemPresenter(
            VotingSystemView view,
            VotingSystemViewModel model,
            GameTableViewModel gameTableViewModel
        )
        {
            _view = view;
            _model = model;
            _gameTableViewModel = gameTableViewModel;
        }

        public void Initialize()
        {
            _view.StartVotingButton.SubscribeOnClick(_model.StartVoting).AddTo(disposables);
            _view.EndVotingButton.SubscribeOnClick(_model.EndVoting).AddTo(disposables);
            _view.ResetInitiatorButton.SubscribeOnClick(_model.ResetInitiator).AddTo(disposables);
            _view.ResetNomineeButton.SubscribeOnClick(_model.ResetNominee).AddTo(disposables);
            _model.CurrentState.Subscribe(UpdateLabel).AddTo(disposables);
            _model.CurrentState.Subscribe(UpdateLabelVisibility).AddTo(disposables);
            _model.CurrentState.Subscribe(UpdateButtonsVisibility).AddTo(disposables);
            
        }

        private void UpdateButtonsVisibility(State state)
        {
            _view.EndVotingButton.SetVisible(state != State.Idle);
            _view.StartVotingButton.SetVisible(state == State.Idle);
            _view.ResetInitiatorButton.SetVisible(state == State.ChoosingNominee);
            _view.ResetNomineeButton.SetVisible(state == State.ChoosingParticipant);
        }

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

        private void UpdateLabelVisibility(State state)
        {
            _view.StateLabel.SetVisible(state != State.Idle);
        }
    }
}
