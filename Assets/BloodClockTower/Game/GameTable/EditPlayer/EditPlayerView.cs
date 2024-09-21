using Nxlk.UIToolkit;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class EditPlayerView
    {
        public Button StartEditingButton { get; private set; }
        public Button EndEditingButton { get; private set; }
        public Button KillPlayerButton { get; private set; }
        public Button RevivePlayerButton { get; private set; }
        public TextField NameInputField { get; private set; }

        public EditPlayerView(SafetyUiDocument safetyUiDocument)
        {
            StartEditingButton = safetyUiDocument.Q<Button>("edit-button");
            EndEditingButton = safetyUiDocument.Q<Button>("end-editing-button");
            KillPlayerButton = safetyUiDocument.Q<Button>("kill-player-button");
            RevivePlayerButton = safetyUiDocument.Q<Button>("revive-player-button");
            NameInputField = safetyUiDocument.Q<TextField>("name-input-field");
        }
    }
}
