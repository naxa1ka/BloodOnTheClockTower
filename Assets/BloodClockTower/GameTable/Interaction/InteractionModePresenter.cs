using BloodClockTower.UI;
using UniRx;

namespace BloodClockTower
{
    public class InteractionModePresenter : DisposableObject,  IPresenter
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
            _view
                .EditToggle.ObserveValue()
                .Subscribe(
                    renameValue =>
                        renameValue.Switch(
                            _model.EnableEditName,
                            _model.DisableEditName
                        )
                )
                .AddTo(disposables);
        }
    }
}