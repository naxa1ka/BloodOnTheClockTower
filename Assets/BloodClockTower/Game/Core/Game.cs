using System;
using System.Collections.Generic;
using System.Linq;
using Nxlk.Bool;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class Game : DisposableObject
    {
        private readonly List<Night> _nights = new();
        private readonly ReactiveProperty<Night> _currentNight;
        private int CurrentNightListIndex => CurrentNight.Value.Number - 1;
        public IReadOnlyReactiveProperty<Night> CurrentNight => _currentNight;

        public Game(int playersAmount)
        {
            var firstNight = new Night(
                Enumerable.Repeat(0, playersAmount).Select(x => new Player())
            );
            _currentNight = new ReactiveProperty<Night>(firstNight);
            _nights.Add(firstNight);
        }

        public void StartNewNight()
        {
            var nextNight = CurrentNight.Value.NextNight();
            _currentNight.Value = nextNight;
            _nights.Add(nextNight);
        }

        public bool IsFirstNight() => CurrentNightListIndex == 0;

        public bool IsLastNight() => CurrentNightListIndex == _nights.Count - 1;

        public void NextNightOrStartNewNight()
        {
            IsLastNight().Switch(StartNewNight, NextNight);
        }

        public void NextNight()
        {
            if (IsLastNight())
                throw new InvalidOperationException();
            _currentNight.Value = _nights[CurrentNightListIndex + 1];
        }

        public void PreviousNight()
        {
            if (IsFirstNight())
                throw new InvalidOperationException();
            _currentNight.Value = _nights[CurrentNightListIndex - 1];
        }
    }
}
