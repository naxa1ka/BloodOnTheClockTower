using System.Diagnostics.Contracts;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class Player : DisposableObject, IPlayer
    {
        private readonly ReactiveProperty<PlayerName> _name;
        private readonly ReactiveProperty<bool> _isAlive;

        public IReadOnlyReactiveProperty<PlayerName> Name => _name;
        public IReadOnlyReactiveProperty<bool> IsAlive => _isAlive;

        public Player()
            : this(PlayerName.From("Unknown")) { }

        public Player(PlayerName name, bool isAlive = true)
        {
            _name = new ReactiveProperty<PlayerName>(name).AddTo(disposables);
            _isAlive = new ReactiveProperty<bool>(isAlive).AddTo(disposables);
        }

        public void ChangeName(string name) => _name.Value = PlayerName.From(name);

        public void Revive() => _isAlive.Value = true;

        public void Kill() => _isAlive.Value = false;

        public IPlayer DeepClone() => new Player(_name.Value, _isAlive.Value);
    }
}
