using Nxlk.UniRx;
using UniRx;
using CollectionExtensions = Nxlk.LINQ.CollectionExtensions;

namespace BloodClockTower.Game
{
    public class VotingHistoryViewModel : DisposableObject, IInitializable
    {
        private readonly ReactiveCollection<VotingRound> _votingRounds;
        private readonly ReactiveProperty<bool> _isVisible;

        public IReadOnlyReactiveCollection<VotingRound> VotingRounds => _votingRounds;
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;

        public VotingHistoryViewModel()
        {
            _isVisible = CollectionExtensions.AddTo(new ReactiveProperty<bool>(), disposables);
            _votingRounds = CollectionExtensions.AddTo(new ReactiveCollection<VotingRound>(), disposables);
        }

        public void Add(VotingRound votingRound)
        {
            _votingRounds.Add(votingRound);
        }

        public void Initialize() { }

        public void Show()
        {
            _isVisible.Value = true;
        }

        public void Hide()
        {
            _isVisible.Value = false;
        }
    }
}
