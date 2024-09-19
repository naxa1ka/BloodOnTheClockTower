using System.Collections.Generic;
using BloodClockTower;
using UniRx;

public class Player : IPlayer
{
    private readonly ReactiveProperty<PlayerName> _name;

    public IReadOnlyReactiveProperty<PlayerName> Name => _name;

    public bool IsAlive { get; private set; } = true;

    public Player()
    {
        _name = new ReactiveProperty<PlayerName>(new PlayerName("Empty name"));
    }

    public void Kill()
    {
        IsAlive = false;
    }

    public void SetName(string name) => _name.Value = new PlayerName(name);
}
