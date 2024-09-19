using BloodClockTower.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using PointerManipulator = BloodClockTower.UI.PointerManipulator;
using VoteRole = BloodClockTower.PlayerViewModel.VoteRole;

namespace BloodClockTower
{
    public class PlayerPresenter : DisposableObject, IPresenter
    {
        private const float InitialSize = 154f;

        private readonly PlayerViewModel _viewModel;
        private readonly PlayerIconView _view;
        private readonly IReadOnlyInteractionMode _interactionMode;

        public PlayerPresenter(
            PlayerViewModel viewModel,
            PlayerIconView view,
            IReadOnlyInteractionMode interactionMode
        )
        {
            _viewModel = viewModel;
            _view = view;
            _interactionMode = interactionMode;
            Disposable.Create(() => _view.Container.RemoveFromHierarchy()).AddTo(disposables);
        }

        public void Initialize()
        {
            _viewModel
                .Name.Subscribe(name => _view.NameLabel.SetValueWithoutNotify(name.ToString()))
                .AddTo(disposables);
            _viewModel
                .Position.Subscribe(newPosition => _view.Container.transform.position = newPosition)
                .AddTo(disposables);
            _viewModel
                .IconSize.Subscribe(size =>
                {
                    var scaledSize = size / InitialSize;
                    _view.Container.transform.scale = new Vector3(scaledSize, scaledSize, 0);
                })
                .AddTo(disposables);
            _viewModel
                .IsSelected.Subscribe(
                    isSelected =>
                        _view.Icon.style.unityBackgroundImageTintColor = new StyleColor(
                            isSelected ? Color.yellow : Color.white
                        )
                )
                .AddTo(disposables);
            _view
                .Container.AddManipulatorWithDispose(
                    new PointerManipulator(
                        onMove: @event =>
                        {
                            if (!_interactionMode.CanDrag.Value)
                                return;
                            _viewModel.ChangePosition(vector3 => vector3 + @event.deltaPosition);
                        },
                        onDown: _ => _viewModel.Click()
                    )
                )
                .AddTo(disposables);
            _viewModel.Role.Subscribe(OnRoleChanged).AddTo(disposables);
        }

        private void OnRoleChanged(VoteRole voteRole)
        {
            _view.BorderInner.SetVisible(voteRole.HasFlag(VoteRole.Participant));
            var isPlayerInitiator = voteRole.HasFlag(VoteRole.Initiator);
            _view.ArrowsInitiator.SetVisible(isPlayerInitiator);
            var isPlayerNominee = voteRole.HasFlag(VoteRole.Nominee);
            _view.ArrowsNominee.SetVisible(isPlayerNominee);
            _view.BorderOuter.SetVisible(isPlayerInitiator || isPlayerNominee);
        }
    }
}
