using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class PlayerNotesView : IPlayerNotesView
    {
        public VisualElement PlayerNotesContainer { get; private set; }
        public Button OpenPlayerNotesButton { get; private set; }
        public Button ClosePlayerNotesButton { get; private set; }
        public Label NoteLabel { get; private set; }
        public MultiLabel PlayerHeaderLabel { get; private set; }
        public TextField NoteInputField { get; private set; }

        public PlayerNotesView(ISafetyUiDocument uiDocument)
        {
            PlayerNotesContainer = uiDocument.Q<VisualElement>("player-notes-container");
            OpenPlayerNotesButton = uiDocument.Q<Button>("open-player-notes-button");
            ClosePlayerNotesButton = PlayerNotesContainer.Q<Button>("close-player-notes-button");
            NoteLabel = PlayerNotesContainer.Q<Label>("note-label");
            PlayerHeaderLabel = PlayerNotesContainer.Q<MultiLabel>("player-header-label");
            NoteInputField = PlayerNotesContainer.Q<TextField>("note-input-field");
        }
    }
}