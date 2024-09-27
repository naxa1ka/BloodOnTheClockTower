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
            _view.Container.RemoveFromHierarchyAsDisposable().AddTo(disposables);
        }

        public void Initialize()
        {
            _viewModel
                .Name.Select(playerName => playerName.ToString())
                .Subscribe(_view.NameLabel.SetValueWithoutNotify)
                .AddTo(disposables);
            _viewModel.Position.BindToTransformPosition(_view.Container).AddTo(disposables);
            _viewModel
                .IconSize.Select(size => size / InitialSize)
                .Select(scaledSize => new Vector3(scaledSize, scaledSize, 0))
                .BindToTransformScale(_view.Container)
                .AddTo(disposables);
            _viewModel
                .IsSelected.Select(isSelected => isSelected ? Color.yellow : Color.white)
                .BindToImageTintColor(_view.Icon)
                .AddTo(disposables);
            _view
                .Container.AddManipulatorWithDispose(
                    new PointerManipulator(onDown: _ => _viewModel.Click())
                )
                .AddTo(disposables);
            _viewModel
                .Role.Select(voteRole => voteRole.IsParticipant)
                .BindToVisible(_view.BorderInner)
                .AddTo(disposables);
            _viewModel
                .Role.Select(voteRole => voteRole.IsInitiator)
                .BindToVisible(_view.ArrowsInitiator)
                .AddTo(disposables);
            _viewModel
                .Role.Select(voteRole => voteRole.IsNominee)
                .BindToVisible(_view.ArrowsNominee)
                .AddTo(disposables);
            _viewModel
                .Role.Select(voteRole => voteRole.IsInitiator || voteRole.IsNominee)
                .BindToVisible(_view.BorderOuter)
                .AddTo(disposables);
            _viewModel.IsAlive.InverseBool().BindToVisible(_view.KilledBorder).AddTo(disposables);
            Observable
                .CombineLatest(
                    _viewModel.IsAlive,
                    _viewModel.HasGhostlyVote,
                    (isAlive, hasGhostlyVote) => !isAlive && hasGhostlyVote
                )
                .BindToVisible(_view.GhostlyVoteIcon)
                .AddTo(disposables);
        }
    }
}
