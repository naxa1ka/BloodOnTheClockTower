using System;
using System.Collections.Generic;
using System.Linq;
using Nxlk.Bool;
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
        private readonly GameTableView _view;
        private readonly GameTableViewModel _viewModel;
        private readonly Game _game;
        private readonly ViewFactory<PlayerIconView> _playerIconViewFactory;
        private readonly Dictionary<
            PlayerViewModel,
            IDisposable
        > _playerViewModelDisposablesMapping;

        private readonly VotingHistoryViewModel _votingHistoryViewModel;

        private bool _isRenaming;
        private PlayerViewModel? Selected =>
            _viewModel.Players.SingleOrDefault(player => player.IsSelected.Value);

        public GameTablePresenter(
            GameTableView view,
            GameTableViewModel viewModel,
            Game game,
            ViewFactory<PlayerIconView> playerIconViewFactory,
            VotingHistoryViewModel votingHistoryViewModel
        )
        {
            _view = view;
            _viewModel = viewModel;
            _game = game;
            _playerIconViewFactory = playerIconViewFactory;
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
                .BindToVisible(_view.Root)
                .AddTo(disposables);
            _viewModel.Clicked.Subscribe(SelectPlayer).AddTo(disposables);
            _view.EditButton.SubscribeOnClick(EditButtonClicked).AddTo(disposables);
            _view
                .NameInputField.ObserveText()
                .Subscribe(name =>
                {
                    if (Selected == null)
                        throw new ArgumentNullException();
                    Selected.ChangeName(name);
                })
                .AddTo(disposables);
            _view
                .Board.RegisterCallbackAsObservable<GeometryChangedEvent>()
                .Subscribe(@event => ArrangePlayersInCircle())
                .AddTo(disposables);

            _view.HeaderLabel.text = $"Night: {_game.CurrentNight.Value.Number}";
            _view
                .NextNightButton.SubscribeOnClick(() => _game.NextNightOrStartNewNight())
                .AddTo(disposables);
            _view.PreviousNightButton.SetEnabled(!_game.IsFirstNight());
            _view
                .PreviousNightButton.SubscribeOnClick(() => _game.PreviousNight())
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
