using Nxlk;
using Nxlk.LINQ;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using PointerManipulator = Nxlk.UIToolkit.PointerManipulator;
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
            CollectionExtensions.AddTo(Disposable.Create(() => _view.Container.RemoveFromHierarchy()), disposables);
        }

        public void Initialize()
        {
            CollectionExtensions.AddTo(_viewModel
                    .Name.Subscribe(name => _view.NameLabel.SetValueWithoutNotify(name.ToString())), disposables);
            CollectionExtensions.AddTo(_viewModel
                    .Position.Subscribe(newPosition => _view.Container.transform.position = newPosition), disposables);
            CollectionExtensions.AddTo(_viewModel
                    .IconSize.Subscribe(size =>
                    {
                        var scaledSize = size / InitialSize;
                        _view.Container.transform.scale = new Vector3(scaledSize, scaledSize, 0);
                    }), disposables);
            CollectionExtensions.AddTo(_viewModel
                    .IsSelected.Subscribe(
                        isSelected =>
                            _view.Icon.style.unityBackgroundImageTintColor = new StyleColor(
                                isSelected ? Color.yellow : Color.white
                            )
                    ), disposables);
            CollectionExtensions.AddTo(_view
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
                    ), disposables);
            CollectionExtensions.AddTo(_viewModel.Role.Subscribe(OnRoleChanged), disposables);
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
