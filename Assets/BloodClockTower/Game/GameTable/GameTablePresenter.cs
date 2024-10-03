using System;
using System.Collections.Generic;
using Nxlk.LINQ;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace BloodClockTower.Game
{
    public class GameTablePresenter : DisposableObject, IPresenter
    {
        private readonly IGameTableView _view;
        private readonly IGameTableViewModel _viewModel;
        private readonly IViewFactory<PlayerIconView> _playerIconViewFactory;
        private readonly Dictionary<
            PlayerViewModel,
            IDisposable
        > _playerViewModelDisposablesMapping;
        private IDisposable _rearrangeSubscription = Disposable.Empty;
        private readonly IVotingHistoryViewModel _votingHistoryViewModel;

        public GameTablePresenter(
            IGameTableView view,
            IGameTableViewModel viewModel,
            IViewFactory<PlayerIconView> playerIconViewFactory,
            IVotingHistoryViewModel votingHistoryViewModel
        )
        {
            _view = view;
            _viewModel = viewModel;
            _playerIconViewFactory = playerIconViewFactory;
            _votingHistoryViewModel = votingHistoryViewModel;
            _playerViewModelDisposablesMapping = new Dictionary<PlayerViewModel, IDisposable>();

            Disposable
                .Create(
                    () =>
                        StableCompositeDisposable
                            .Create(
                                _playerViewModelDisposablesMapping.Values.Concat(
                                    _rearrangeSubscription
                                )
                            )
                            .Dispose()
                )
                .AddTo(disposables);
        }

        public void Initialize()
        {
            _viewModel
                .Players.ObserveAddItemWithCollection()
                .Subscribe(AddPlayer)
                .AddTo(disposables);
            _viewModel.Players.ObserveRemoveItem().Subscribe(RemovePlayer).AddTo(disposables);
            _rearrangeSubscription = _view
                .Board.RegisterCallbackAsObservable<GeometryChangedEvent>()
                .Subscribe(RearrangePlayersInCircle);
            _votingHistoryViewModel
                .IsVisible.InverseBool()
                .BindToVisible(_view.Root)
                .AddTo(disposables);
        }

        private void RearrangePlayersInCircle(GeometryChangedEvent @event)
        {
            ArrangePlayersInCircle();
            _rearrangeSubscription.Dispose();
            _rearrangeSubscription = Disposable.Empty;
        }

        private void AddPlayer(PlayerViewModel playerViewModel)
        {
            var playerIconView = _playerIconViewFactory.Create();
            _view.Board.Add(playerIconView.Container);
            var playerPresenter = new PlayerPresenter(playerViewModel, playerIconView);
            playerPresenter.Initialize();
            _playerViewModelDisposablesMapping.Add(playerViewModel, playerPresenter);
            ArrangePlayersInCircle();
        }

        private void RemovePlayer(PlayerViewModel playerViewModel)
        {
            _playerViewModelDisposablesMapping[playerViewModel].Dispose();
            _playerViewModelDisposablesMapping.Remove(playerViewModel);
        }

        private void ArrangePlayersInCircle()
        {
            const float maxIconSizeLimit = 256f;
            const float leftMargin = 0f;
            const float rightMargin = 0f;
            const float topMargin = 0f;
            const float bottomMargin = 0f;

            var playerCount = _viewModel.Players.Count;
            if (playerCount == 0)
                return;

            var boardWidth = _view.Board.resolvedStyle.width;
            var boardHeight = _view.Board.resolvedStyle.height;

            var effectiveWidth = boardWidth - leftMargin - rightMargin;
            var effectiveHeight = boardHeight - topMargin - bottomMargin;
            var centerX = leftMargin + effectiveWidth / 2f;
            var centerY = bottomMargin + effectiveHeight / 2f;

            var sinHalfAngle = Mathf.Sin(Mathf.PI / playerCount);

            var k = 1f / sinHalfAngle + 1f;

            var sWidth = effectiveWidth / k;
            var sHeight = effectiveHeight / k;
            var sMax = Mathf.Min(sWidth, sHeight, maxIconSizeLimit);

            var r = sMax / (2f * sinHalfAngle);

            var rMaxX = effectiveWidth / 2f - sMax / 2f;
            var rMaxY = effectiveHeight / 2f - sMax / 2f;
            var rMax = Mathf.Min(rMaxX, rMaxY);

            if (r > rMax)
            {
                r = rMax;
                sMax = 2f * r * sinHalfAngle;
                sMax = Mathf.Min(sMax, maxIconSizeLimit);
            }

            var angleStepRadians = 2f * Mathf.PI / playerCount;
            const float startAngle = -Mathf.PI / 2f;

            for (var index = 0; index < playerCount; index++)
            {
                var playerViewModel = _viewModel.Players[index];
                var angle = startAngle + index * angleStepRadians;
                var x = centerX + r * Mathf.Cos(angle) - sMax / 2f;
                var y = centerY + r * Mathf.Sin(angle) - sMax / 2f;

                playerViewModel.SetPosition(new Vector3(x, y, 0f));
                playerViewModel.SetSize(sMax);
            }
        }
    }
}
