﻿using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class VotingSystemView : IVotingSystemView
    {
        public Button StartVotingButton { get; private set; }
        public Button EndVotingButton { get; private set; }
        public Button ResetInitiatorButton { get; private set; }
        public Button ResetNomineeButton { get; private set; }
        public Label StateLabel { get; private set; }
        public VisualElement StateLabelContainer { get; private set; }

        public VotingSystemView(ISafetyUiDocument uiDocument)
        {
            StartVotingButton = uiDocument.Q<Button>("start-voting-button");
            EndVotingButton = uiDocument.Q<Button>("end-voting-button");
            ResetInitiatorButton = uiDocument.Q<Button>("reset-initiator-button");
            ResetNomineeButton = uiDocument.Q<Button>("reset-nominee-button");
            StateLabel = uiDocument.Q<Label>("state-label");
            StateLabelContainer = uiDocument.Q<VisualElement>("state-label-container");
        }
    }
}
