﻿using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class NightChangingPresenter : DisposableObject, IPresenter
    {
        private readonly INightChangingView _view;
        private readonly IGame _game;

        public NightChangingPresenter(INightChangingView view, IGame game)
        {
            _view = view;
            _game = game;
        }

        public void Initialize()
        {
            _view.NightCountLabel.text = $"Night: {_game.CurrentNight.Value.Number}";
            _view
                .NextNightButton.SubscribeOnClick(() => _game.NextNightOrStartNewNight())
                .AddTo(disposables);
            _view.PreviousNightButton.SetEnabled(!_game.IsFirstNight());
            _view
                .PreviousNightButton.SubscribeOnClick(() => _game.PreviousNight())
                .AddTo(disposables);
        }
    }
}
