using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class PlayerStatus : DisposableObject, IPlayerStatus
    {
        private readonly ReactiveProperty<bool> _isAlive;
        private readonly ReactiveProperty<bool> _hasGhostlyVote;
        private readonly ReactiveProperty<string> _note;
        private readonly IPlayer _player;
        
        public IReadOnlyReactiveProperty<PlayerName> Name => _player.Name;
        public IReadOnlyReactiveProperty<bool> IsAlive => _isAlive;
        public IReadOnlyReactiveProperty<bool> HasGhostlyVote => _hasGhostlyVote;
        public IReadOnlyReactiveProperty<string> Note => _note;
        public IPlayer Original => _player;

        public PlayerStatus(IPlayer player, bool isAlive = true)
        {
            _player = player;
            _isAlive = new ReactiveProperty<bool>(isAlive).AddTo(disposables);
            _hasGhostlyVote = new ReactiveProperty<bool>(true).AddTo(disposables);
            _note = new ReactiveProperty<string>(string.Empty).AddTo(disposables);
        }

        public void ChangeName(string name) => _player.ChangeName(name);
        
        public void ChangeNote(string note) => _note.Value = note;

        public void UseGhostlyVoice() => _hasGhostlyVote.Value = false;

        public void Kill() => _isAlive.Value = false;

        public IPlayerStatus DeepClone() => new PlayerStatus(_player, _isAlive.Value);
    }
}
