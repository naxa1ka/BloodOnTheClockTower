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

        public VotingSystemPresenter(VotingSystemView view, VotingSystemViewModel model)
        {
            _view = view;
            _model = model;
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
