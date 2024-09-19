using Nxlk.Bool;
using Nxlk.LINQ;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class InteractionModePresenter : DisposableObject, IPresenter
    {
        private readonly InteractionModeView _view;
        private readonly IInteractionMode _model;

        public InteractionModePresenter(InteractionModeView view, IInteractionMode model)
        {
            _view = view;
            _model = model;
        }

        public void Initialize()
        {
            CollectionExtensions.AddTo(_view
                    .EditToggle.ObserveValue()
                    .Subscribe(
                        renameValue => renameValue.Switch(_model.EnableEditName, _model.DisableEditName)
                    ), disposables);
        }
    }
}
