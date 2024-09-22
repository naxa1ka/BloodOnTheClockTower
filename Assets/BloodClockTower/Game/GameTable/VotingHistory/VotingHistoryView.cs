using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class VotingHistoryView
    {
        public Button CloseVotingHistoryButton { get; private set; }
        public Button OpenVotingHistoryButton { get; private set; }
        public Button EditNoteVotingHistoryButton { get; private set; }
        public Button DoneNoteVotingHistoryButton { get; private set; }
        public VisualElement VotingHistoryContainer { get; private set; }
        public Label VotingHistoryLabel { get; private set; }
        public Label NoteLabel { get; private set; }
        public TextField NoteInputField { get; private set; }
        
        public VotingHistoryView(SafetyUiDocument uiDocument)
        {
            CloseVotingHistoryButton = uiDocument.Q<Button>("close-voting-history-button");
            OpenVotingHistoryButton = uiDocument.Q<Button>("open-voting-history-button");
            EditNoteVotingHistoryButton = uiDocument.Q<Button>("edit-note-voting-history-button");
            DoneNoteVotingHistoryButton = uiDocument.Q<Button>("done-note-voting-history-button");
            VotingHistoryContainer = uiDocument.Q<VisualElement>("voting-history-container");
            VotingHistoryLabel = uiDocument.Q<Label>("voting-history-label");
            NoteLabel = uiDocument.Q<Label>("note-label");
            NoteInputField = uiDocument.Q<TextField>("note-input-field");
        }
    }
}
