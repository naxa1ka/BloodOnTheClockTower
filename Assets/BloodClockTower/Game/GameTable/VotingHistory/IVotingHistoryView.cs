using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public interface IVotingHistoryView
    {
        Button CloseVotingHistoryButton { get; }
        Button OpenVotingHistoryButton { get; }
        VisualElement VotingHistoryContainer { get; }
        Label VotingHistoryLabel { get; }
        TextField NoteInputField { get; }
    }
}