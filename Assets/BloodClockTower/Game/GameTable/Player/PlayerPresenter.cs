using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using PointerManipulator = Nxlk.UIToolkit.PointerManipulator;

namespace BloodClockTower.Game
{
    public class PlayerPresenter : DisposableObject, IPresenter
    {
        private const float InitialSize = 154f;

        private readonly PlayerViewModel _viewModel;
        private readonly PlayerIconView _view;

        public PlayerPresenter(PlayerViewModel viewModel, PlayerIconView view)
        {
            _viewModel = viewModel;
            _view = view;
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
                    new PointerManipulator(onDown: _ => _viewModel.Click())
                )
                .AddTo(disposables);
            _viewModel.Role.Subscribe(OnRoleChanged).AddTo(disposables);
            _viewModel.IsAlive.InverseBool().BindToVisible(_view.KilledBorder).AddTo(disposables);
        }

        private void OnRoleChanged(VoteRole voteRole)
        {
            _view.BorderInner.SetVisible(voteRole.IsParticipant);
            _view.ArrowsInitiator.SetVisible(voteRole.IsInitiator);
            _view.ArrowsNominee.SetVisible(voteRole.IsNominee);
            _view.BorderOuter.SetVisible(voteRole.IsInitiator || voteRole.IsNominee);
        }
    }
}
