using System.Collections.Generic;
using UniRx;

namespace BloodClockTower
{
    public class VotingHistoryViewModel : DisposableObject, IInitializable
    {
        private readonly ReactiveCollection<VotingRound> _votingRounds;
        private readonly ReactiveProperty<bool> _isVisible;

        public IReadOnlyReactiveCollection<VotingRound> VotingRounds => _votingRounds;
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;

        public VotingHistoryViewModel()
        {
            _isVisible = new ReactiveProperty<bool>().AddTo(disposables);
            _votingRounds = new ReactiveCollection<VotingRound>().AddTo(disposables);
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
