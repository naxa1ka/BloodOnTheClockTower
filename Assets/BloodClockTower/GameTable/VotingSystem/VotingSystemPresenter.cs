using System;
using System.Diagnostics;
using Nxlk;
using Nxlk.LINQ;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
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
            CollectionExtensions.AddTo(_view.StartVotingButton.SubscribeOnClick(_model.StartVoting), disposables);
            CollectionExtensions.AddTo(_view.EndVotingButton.SubscribeOnClick(_model.EndVoting), disposables);
            CollectionExtensions.AddTo(_view.ResetInitiatorButton.SubscribeOnClick(_model.ResetInitiator), disposables);
            CollectionExtensions.AddTo(_view.ResetNomineeButton.SubscribeOnClick(_model.ResetNominee), disposables);
            CollectionExtensions.AddTo(_model.CurrentState.Subscribe(UpdateLabel), disposables);
            CollectionExtensions.AddTo(_model.CurrentState.Subscribe(UpdateLabelVisibility), disposables);
            CollectionExtensions.AddTo(_model.CurrentState.Subscribe(UpdateButtonsVisibility), disposables);
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
