using BloodClockTower.UI;
using UnityEngine.UIElements;

namespace BloodClockTower
{
    public class VotingHistoryView
    {
        public Button VotingHistoryButton { get; private set; }
        public VisualElement VotingHistoryContainer { get; private set; }
        public Label VotingHistoryLabel { get; private set; }

        public VotingHistoryView(SafetyUiDocument uiDocument)
        {
            VotingHistoryButton = uiDocument.Q<Button>("voting-history-button");
            VotingHistoryContainer = uiDocument.Q<VisualElement>("voting-history-container");
            VotingHistoryLabel = uiDocument.Q<Label>("voting-history-label");
        }
    }
}
