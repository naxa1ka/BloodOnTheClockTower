using BloodClockTower.Game;
using Cysharp.Threading.Tasks;
using Nxlk.LINQ;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;

namespace BloodClockTower.Menu
{
    public class MenuPresenter : DisposableObject, IPresenter
    {
        private readonly MenuView _view;
        private readonly StartGameCommand _startGameCommand;

        public MenuPresenter(MenuView view, StartGameCommand startGameCommand)
        {
            _startGameCommand = startGameCommand;
            _view = view;
        }

        public void Initialize()
        {
            _view.StartButton.SubscribeOnClick(() => Start().Forget()).AddTo(disposables);
        }

        private async UniTaskVoid Start()
        {
            await _startGameCommand.Execute(_view.PlayersAmountInputField.value);
        }
    }
}
