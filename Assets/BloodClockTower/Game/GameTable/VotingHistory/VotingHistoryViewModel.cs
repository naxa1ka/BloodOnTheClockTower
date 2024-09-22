using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class VotingHistoryViewModel : DisposableObject, IInitializable
    {
        private readonly Night _night;
        private readonly ReactiveProperty<bool> _isVisible;
        private readonly ReactiveProperty<bool> _isEditingNote;

        public VotingRoundsPerNight VotingRounds => _night.VotingRounds;
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        public IReadOnlyReactiveProperty<bool> IsEditingNote => _isEditingNote;
        
        public VotingHistoryViewModel(Night night)
        {
            _night = night;
            _isVisible = new ReactiveProperty<bool>(false).AddTo(disposables);
            _isEditingNote = new ReactiveProperty<bool>(false).AddTo(disposables);
        }

        public void Initialize() { }

        public void Show() => _isVisible.Value = true;
        
        public void EndEditingNote()
        {
            _isEditingNote.Value = false;
            _night.VotingRounds.SetDefaultNoteIfItIsEmpty();
        }

        public void StartEditingNote() => _isEditingNote.Value = true;
        
        public void Hide()
        {
            _isVisible.Value = false;
            EndEditingNote();
        }

        public void ChangeNote(string note) => _night.VotingRounds.ChangeNote(note);

        public void Add(IVotingRound votingRound) => _night.VotingRounds.Add(votingRound);
    }
}
