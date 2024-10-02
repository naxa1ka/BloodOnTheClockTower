using UniRx;

namespace BloodClockTower.Game
{
    public interface IVotingSystemViewModel
    {
        IReadOnlyReactiveProperty<VotingSystemState> CurrentState { get; }
        void StartVoting();
        void EndVoting();
        void ResetInitiator();
        void ResetNominee();
    }
}