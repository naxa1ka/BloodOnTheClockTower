using BloodClockTower.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace BloodClockTower
{
    public class GameTableView
    {
        public VisualElement Board { get; private set; }
        public Button EditButton { get; private set; }
        public TextField NameInputField { get; private set; }
        
        public GameTableView(SafetyUiDocument safetyUiDocument)
        {
            Board = safetyUiDocument.Q<VisualElement>("board");
            EditButton = safetyUiDocument.Q<Button>("edit-button");
            NameInputField = safetyUiDocument.Q<TextField>("name-input-field");
        }
    }
}
