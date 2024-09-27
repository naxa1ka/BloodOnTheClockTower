using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class Player : DisposableObject, IPlayer
    {
        private readonly ReactiveProperty<PlayerName> _name;

        public IReadOnlyReactiveProperty<PlayerName> Name => _name;

        public Player(string name)
            : this(PlayerName.From(name)) { }

        public Player(PlayerName name)
        {
            _name = new ReactiveProperty<PlayerName>(name).AddTo(disposables);
        }

        public void ChangeName(string name) => _name.Value = PlayerName.From(name);
    }
}
