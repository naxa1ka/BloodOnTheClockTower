using UniRx;

namespace BloodClockTower.Game
{
    public interface IVotingHistoryViewModel
    {
        VotingRoundsPerNight VotingRounds { get; }
        IReadOnlyReactiveProperty<bool> IsVisible { get; }
        void Show();
        void EndEditingNote();
        void Hide();
        void ChangeNote(string note);
        void Add(IVotingRound votingRound);
    }
}