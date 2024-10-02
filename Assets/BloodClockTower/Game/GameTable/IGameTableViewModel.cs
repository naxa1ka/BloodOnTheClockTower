using System;
using UniRx;

namespace BloodClockTower.Game
{
    public interface IGameTableViewModel
    {
        IReadOnlyReactiveCollection<PlayerViewModel> Players { get; }
        IObservable<PlayerViewModel> Clicked { get; }
    }
}