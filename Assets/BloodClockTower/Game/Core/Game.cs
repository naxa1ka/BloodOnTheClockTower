using System;
using System.Collections.Generic;
using System.Linq;
using Nxlk.Bool;
using Nxlk.LINQ;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class Game : DisposableObject
    {
        private readonly List<Night> _nights = new();
        private readonly List<IPlayer> _players = new();
        private readonly ReactiveProperty<Night> _currentNight;

        private int CurrentNightListIndex => CurrentNight.Value.Number - 1;
        public IReadOnlyReactiveProperty<Night> CurrentNight => _currentNight;

        public Game(int playersAmount)
        {
            for (var i = 0; i < playersAmount; i++)
                _players.Add(new Player($"player-{i}"));
            var firstNight = new Night(_players.Select(player => new PlayerStatus(player)));
            _currentNight = new ReactiveProperty<Night>(firstNight).AddTo(disposables);
            _nights.Add(firstNight);
            Disposable.Create(() => _nights.ForEach(night => night.Dispose())).AddTo(disposables);
        }

        public string GetNotes(IPlayer player)
        {
            return string.Join(
                "\n",
                _nights.Except(_currentNight.Value).Select(night =>
                {
                    var playerStatus = night.Players.Single(
                        playerStatus => playerStatus.Original == player
                    );
                    return $"Night {night.Number}:\n {playerStatus.Note.Value}";
                })
            );
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
