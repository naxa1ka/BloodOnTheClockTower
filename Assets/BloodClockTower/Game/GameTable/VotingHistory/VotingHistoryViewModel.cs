using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class VotingHistoryViewModel : DisposableObject, IInitializable
    {
        private readonly Night _night;
        private readonly ReactiveProperty<bool> _isVisible;

        public IReadOnlyReactiveCollection<IVotingRound> VotingRounds => _night.VotingRounds;
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;

        public VotingHistoryViewModel(Night night)
        {
            _night = night;
            _isVisible = new ReactiveProperty<bool>().AddTo(disposables);
        }

        public void Initialize() { }

        public void Add(IVotingRound votingRound) => _night.Add(votingRound);

        public void Show() => _isVisible.Value = true;

        public void Hide() => _isVisible.Value = false;
    }
}
