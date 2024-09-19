using Nxlk.UIToolkit;

namespace BloodClockTower.Menu
{
    public class MenuPresenter : IPresenter
    {
        private readonly MenuView _view;
        private readonly MenuViewModel _viewModel;

        public MenuPresenter(MenuView view, MenuViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            _view.StartButton.clicked += Start;
        }

        private void Start()
        {
            _viewModel.StartGame(_view.PlayersAmountInputField.value);
        }

        public void Dispose()
        {
            _view.StartButton.clicked -= Start;
        }
    }
}
