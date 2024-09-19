using System;
using System.Collections.Generic;
using System.Linq;
using BloodClockTower.UI;
using OneOf;
using OneOf.Types;
using UniRx;
using UnityEngine;

namespace BloodClockTower
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
            Disposable
                .Create(
                    () =>
                        StableCompositeDisposable
                            .Create(_playerViewModelDisposablesMapping.Values)
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

            _votingHistoryViewModel
                .IsVisible.InverseBool()
                .BindToVisible(_view.Board)
                .AddTo(disposables);
            _viewModel.Clicked.Subscribe(SelectPlayer).AddTo(disposables);
            _view.EditButton.SubscribeOnClick(EditButtonClicked).AddTo(disposables);
            _view
                .NameInputField.ObserveText()
                .Subscribe(name =>
                {
                    if (Selected == null)
                        throw new ArgumentNullException();
                    Selected.SetName(name);
                })
                .AddTo(disposables);
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
            const float leftMargin = 0f; // Left margin in pixels
            const float rightMargin = 0f; // Right margin in pixels
            const float topMargin = 0f; // Top margin in pixels
            const float bottomMargin = 0f; // Bottom margin in pixels

            int playerCount = _viewModel.Players.Count;
            if (playerCount == 0)
                return;

            var boardWidth = _view.Board.resolvedStyle.width;
            var boardHeight = _view.Board.resolvedStyle.height;

            // Calculate effective dimensions
            var effectiveWidth = boardWidth - leftMargin - rightMargin;
            var effectiveHeight = boardHeight - topMargin - bottomMargin;
            var centerX = leftMargin + (effectiveWidth / 2f);
            var centerY = bottomMargin + (effectiveHeight / 2f);

            // Calculate sin(π / N) once for efficiency
            float sinHalfAngle = Mathf.Sin(Mathf.PI / playerCount);

            // Calculate K factor
            float K = (1f / sinHalfAngle) + 1f;

            // Compute maximum icon sizes based on effective dimensions
            float S_width = effectiveWidth / K;
            float S_height = effectiveHeight / K;
            float S_max = Mathf.Min(S_width, S_height, maxIconSizeLimit);

            // Calculate initial radius
            float R = S_max / (2f * sinHalfAngle);

            // Compute maximum possible radius considering icon size
            float R_max_X = (effectiveWidth / 2f) - (S_max / 2f);
            float R_max_Y = (effectiveHeight / 2f) - (S_max / 2f);
            float R_max = Mathf.Min(R_max_X, R_max_Y);

            // Adjust radius and icon size if necessary
            if (R > R_max)
            {
                R = R_max;
                S_max = 2f * R * sinHalfAngle;
                // Ensure S_max does not exceed maxIconSizeLimit
                S_max = Mathf.Min(S_max, maxIconSizeLimit);
            }

            // Angle between each icon in radians
            float angleStepRadians = 2f * Mathf.PI / playerCount;
            float startAngle = -Mathf.PI / 2f; // Start from the top

            for (int index = 0; index < playerCount; index++)
            {
                var playerViewModel = _viewModel.Players[index];
                float angle = startAngle + index * angleStepRadians;
                float x = centerX + R * Mathf.Cos(angle) - S_max / 2f;
                float y = centerY + R * Mathf.Sin(angle) - S_max / 2f;

                playerViewModel.SetPosition(new Vector3(x, y, 0f));
                playerViewModel.SetSize(S_max);
            }
        }
    }
}
