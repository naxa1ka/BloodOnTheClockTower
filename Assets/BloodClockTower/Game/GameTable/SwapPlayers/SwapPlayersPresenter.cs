using Nxlk.Bool;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class SwapPlayersPresenter : DisposableObject, IPresenter
    {
        private readonly ISwapPlayersView _view;
        private readonly ISwapPlayersViewModel _viewModel;

        public SwapPlayersPresenter(ISwapPlayersView view, ISwapPlayersViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            _view
                .SwapPlayersButton.SubscribeOnClick(
                    () =>
                        _viewModel.IsEditing.Value.Switch(
                            _viewModel.EndEditing,
                            _viewModel.StartEditing
                        )
                )
                .AddTo(disposables);
        }
    }
}
