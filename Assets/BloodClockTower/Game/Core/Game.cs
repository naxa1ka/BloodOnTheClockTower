using System;
using System.Collections.Generic;
using System.Linq;
using Nxlk.Bool;
using Nxlk.Initialization;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class Game : DisposableObject, IInitializable
    {
        private readonly List<Night> _nights = new();
        private readonly List<IPlayer> _players = new();
        private readonly ReactiveProperty<Night> _currentNight;
        private readonly IChangeNightCommand _changeNightCommand;

        private int CurrentNightListIndex => CurrentNight.Value.Number - 1;
        public IReadOnlyReactiveProperty<Night> CurrentNight => _currentNight;

        public Game(GamePlayersAmount playersAmount, IChangeNightCommand changeNightCommand)
        {
            _changeNightCommand = changeNightCommand;
            for (var i = 0; i < playersAmount.Value; i++)
                _players.Add(new Player($"player-{i}"));
            var firstNight = new Night(_players.Select(player => new PlayerStatus(player)));
            _currentNight = new ReactiveProperty<Night>(firstNight).AddTo(disposables);
            _nights.Add(firstNight);
            Disposable.Create(() => _nights.ForEach(night => night.Dispose())).AddTo(disposables);
        }

        public void Initialize()
        {
            _changeNightCommand.Execute(_currentNight.Value);
        }
        
        public void StartNewNight()
        {
            var nextNight = CurrentNight.Value.NextNight();
            SetNight(nextNight);
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
            SetNight(_nights[CurrentNightListIndex + 1]);
        }

        public void PreviousNight()
        {
            if (IsFirstNight())
                throw new InvalidOperationException();
            SetNight(_nights[CurrentNightListIndex - 1]);
        }

        private void SetNight(Night night)
        {
            _currentNight.Value = night;
            _changeNightCommand.Execute(night);
        }
    }
}
