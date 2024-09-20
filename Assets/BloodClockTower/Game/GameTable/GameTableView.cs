using Nxlk.UIToolkit;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace BloodClockTower.Game
{
    public class GameTableView
    {
        public VisualElement Root { get; private set; }
        public Label HeaderLabel { get; private set; }
        public VisualElement Board { get; private set; }
        public Button EditButton { get; private set; }
        public TextField NameInputField { get; private set; }
        public Button NextNightButton { get; private set; }
        public Button PreviousNightButton { get; private set; }

        public GameTableView(SafetyUiDocument safetyUiDocument)
        {
            Root = safetyUiDocument.Q<VisualElement>("root");
            HeaderLabel = safetyUiDocument.Q<Label>("header-label");
            Board = safetyUiDocument.Q<VisualElement>("board");
            EditButton = safetyUiDocument.Q<Button>("edit-button");
            NameInputField = safetyUiDocument.Q<TextField>("name-input-field");
            NextNightButton = safetyUiDocument.Q<Button>("next-night-button");
            PreviousNightButton = safetyUiDocument.Q<Button>("prev-night-button");
        }
    }
}
