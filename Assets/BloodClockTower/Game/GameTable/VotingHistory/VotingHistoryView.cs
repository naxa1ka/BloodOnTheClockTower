using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class VotingHistoryView
    {
        public VisualElement VotingHistoryContainer { get; private set; }
        public Button OpenVotingHistoryButton { get; private set; }
        public Button CloseVotingHistoryButton { get; private set; }
        public Label VotingHistoryLabel { get; private set; }
        public TextField NoteInputField { get; private set; }

        public VotingHistoryView(SafetyUiDocument uiDocument)
        {
            OpenVotingHistoryButton = uiDocument.Q<Button>("open-voting-history-button");
            VotingHistoryContainer = uiDocument.Q<VisualElement>("voting-history-container");
            CloseVotingHistoryButton = VotingHistoryContainer.Q<Button>("close-voting-history-button");
            VotingHistoryLabel = VotingHistoryContainer.Q<Label>("voting-history-label");
            NoteInputField = VotingHistoryContainer.Q<TextField>("note-input-field");
        }
    }
}
