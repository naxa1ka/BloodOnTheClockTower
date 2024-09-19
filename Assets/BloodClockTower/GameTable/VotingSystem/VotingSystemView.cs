using BloodClockTower.UI;
using UnityEngine.UIElements;

namespace BloodClockTower
{
    public class VotingSystemView
    {
        public Button StartVotingButton { get; private set; }
        public Button EndVotingButton { get; private set; }
        public Button ResetInitiatorButton { get; private set; }
        public Button ResetNomineeButton { get; private set; }
        public Label StateLabel { get; private set; }

        public VotingSystemView(SafetyUiDocument uiDocument)
        {
            StartVotingButton = uiDocument.Q<Button>("start-voting-button");
            EndVotingButton = uiDocument.Q<Button>("end-voting-button");
            ResetInitiatorButton = uiDocument.Q<Button>("reset-initiator-button");
            ResetNomineeButton = uiDocument.Q<Button>("reset-nominee-button");
            StateLabel = uiDocument.Q<Label>("state-label");
        }
    }
}