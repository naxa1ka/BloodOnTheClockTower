using System.Diagnostics.Contracts;
using UniRx;

namespace BloodClockTower.Game
{
    public class Player : IPlayer
    {
        private readonly ReactiveProperty<PlayerName> _name;

        public IReadOnlyReactiveProperty<PlayerName> Name => _name;
        public bool IsAlive { get; }

        public Player()
            : this(PlayerName.From("Unknown")) { }

        public Player(PlayerName name, bool isAlive = true)
        {
            _name = new ReactiveProperty<PlayerName>(name);
            IsAlive = isAlive;
        }

        public void ChangeName(string name) => _name.Value = PlayerName.From(name);

        public IPlayer DeepClone() => new Player(_name.Value, IsAlive);
    }
}
