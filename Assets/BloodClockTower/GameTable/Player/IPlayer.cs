using System.Collections.Generic;
using BloodClockTower;
using UniRx;

public interface IPlayer
{
    IReadOnlyReactiveProperty<PlayerName> Name { get; }
    bool IsAlive { get; }
    void Kill();
    void SetName(string name);
}
