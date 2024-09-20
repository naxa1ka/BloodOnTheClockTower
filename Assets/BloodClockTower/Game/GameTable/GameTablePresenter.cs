using System;
using System.Collections.Generic;
using System.Linq;
using Nxlk.Bool;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;
using UnityEngine;
using CollectionExtensions = Nxlk.LINQ.CollectionExtensions;

namespace BloodClockTower.Game
{
    public class GameTablePresenter : DisposableObject, IInitializable
    {
        private readonly GameTableView _view;
        private readonly GameTableViewModel _viewModel;
        private readonly ViewFactory<PlayerIconView> _playerIconViewFactory;
        private readonly Dictionary<
            PlayerViewModel,
            IDisposable
        > _playerViewModelDisposablesMapping;
        private readonly IReadOnlyInteractionMode _interactionMode;
        private readonly VotingHistoryViewModel _votingHistoryViewModel;

        private bool _isRenaming;
        private PlayerViewModel? Selected =>
            _viewModel.Players.SingleOrDefault(player => player.IsSelected.Value);

        public GameTablePresenter(
            GameTableView view,
            GameTableViewModel viewModel,
            ViewFactory<PlayerIconView> playerIconViewFactory,
            IReadOnlyInteractionMode interactionMode,
            VotingHistoryViewModel votingHistoryViewModel
        )
        {
            _view = view;
            _viewModel = viewModel;
            _playerIconViewFactory = playerIconViewFactory;
            _interactionMode = interactionMode;
            _votingHistoryViewModel = votingHistoryViewModel;
            _playerViewModelDisposablesMapping = new Dictionary<PlayerViewModel, IDisposable>();
            CollectionExtensions.AddTo(Disposable
                    .Create(
                        () =>
                            StableCompositeDisposable
                                .Create(_playerViewModelDisposablesMapping.Values)
                                .Dispose()
                    ), disposables);
        }

        public void Initialize()
        {
            CollectionExtensions.AddTo(_viewModel
                    .Players.ObserveAddItemWithCollection()
                    .Subscribe(AddPlayer), disposables);
            CollectionExtensions.AddTo(_viewModel.Players.ObserveRemoveItem().Subscribe(RemovePlayer), disposables);

            CollectionExtensions.AddTo(_votingHistoryViewModel
                    .IsVisible.InverseBool()
                    .BindToVisible(_view.Board), disposables);
            CollectionExtensions.AddTo(_viewModel.Clicked.Subscribe(SelectPlayer), disposables);
            CollectionExtensions.AddTo(_view.EditButton.SubscribeOnClick(EditButtonClicked), disposables);
            CollectionExtensions.AddTo(_view
                    .NameInputField.ObserveText()
                    .Subscribe(name =>
                    {
                        if (Selected == null)
                            throw new ArgumentNullException();
                        Selected.SetName(name);
                    }), disposables);
        }

        private void SelectPlayer(PlayerViewModel model)
        {
            _isRenaming.Switch(
                () =>
                {
                    _view.NameInputField.Show();
                    _view.NameInputField.SetValueWithoutNotify(model.Name.Value.ToString());
                    Selected?.Deselect();
                    model.Select();
                },
                () => { }
            );
        }

        private void EditButtonClicked()
        {
            _isRenaming.Switch(
                () =>
                {
                    _view.NameInputField.Hide();
                    Selected?.Deselect();
                    _isRenaming = false;
                },
                () =>
                {
                    if (Selected != null)
                        _view.NameInputField.Show();
                    _isRenaming = true;
                }
            );
        }

        private void AddPlayer(PlayerViewModel playerViewModel)
        {
            var playerIconView = _playerIconViewFactory.Create();
            _view.Board.Add(playerIconView.Container);
            var playerPresenter = new PlayerPresenter(
                playerViewModel,
                playerIconView,
                _interactionMode
            );
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
