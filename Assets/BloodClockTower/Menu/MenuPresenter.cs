using BloodClockTower.Game;
using Nxlk.LINQ;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;

namespace BloodClockTower.Menu
{
    public class MenuPresenter : DisposableObject, IPresenter
    {
        private readonly IMenuView _view;
        private readonly StartGameCommand _startGameCommand;

        public MenuPresenter(IMenuView view, StartGameCommand startGameCommand)
        {
            _startGameCommand = startGameCommand;
            _view = view;
        }

        public void Initialize()
        {
            _view
                .StartButton.SubscribeOnClick(
                    () => _startGameCommand.Execute(_view.PlayersAmountInputField.value)
                )
                .AddTo(disposables);
        }
    }
}
