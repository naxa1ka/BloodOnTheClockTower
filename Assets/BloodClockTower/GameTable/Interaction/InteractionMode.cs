using Nxlk.LINQ;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower
{
    public class InteractionMode : DisposableObject, IInteractionMode
    {
        private readonly ReactiveProperty<bool> _canDrag;
        private readonly ReactiveProperty<bool> _canEditName;

        public IReadOnlyReactiveProperty<bool> CanDrag => _canDrag;
        public IReadOnlyReactiveProperty<bool> CanEditName => _canEditName;

        public InteractionMode()
        {
            _canDrag = CollectionExtensions.AddTo(new ReactiveProperty<bool>(false), disposables);
            _canEditName = CollectionExtensions.AddTo(new ReactiveProperty<bool>(false), disposables);
        }

        public void EnableEditName() => _canEditName.Value = true;

        public void DisableEditName() => _canEditName.Value = false;

        public void EnableDrag() => _canDrag.Value = true;

        public void DisableDrag() => _canDrag.Value = false;
    }
}
