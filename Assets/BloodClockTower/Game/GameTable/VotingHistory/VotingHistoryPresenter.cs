using System.Linq;
using Nxlk.Bool;
using Nxlk.LINQ;
using Nxlk.ReactiveUIToolkit;
using Nxlk.UIToolkit;
using Nxlk.UniRx;
using UniRx;

namespace BloodClockTower.Game
{
    public class VotingHistoryPresenter : DisposableObject, IPresenter
    {
        private readonly VotingHistoryView _view;
        private readonly VotingHistoryViewModel _viewModel;
        private readonly VotingSystemViewModel _votingSystemViewModel;

        public VotingHistoryPresenter(
            VotingHistoryView view,
            VotingHistoryViewModel viewModel,
            VotingSystemViewModel votingSystemViewModel
        )
        {
            _view = view;
            _viewModel = viewModel;
            _votingSystemViewModel = votingSystemViewModel;
        }

        public void Initialize()
        {
            CollectionExtensions.AddTo(Observable
                    .CombineLatest(
                        _viewModel.VotingRounds.ObserveCountChanged(),
                        _votingSystemViewModel.CurrentState,
                        (votingRoundsCount, state) =>
                            votingRoundsCount > 0 && state == VotingSystemViewModel.State.Idle
                    )
                    .StartWith(false)
                    .BindToVisible(_view.VotingHistoryButton), disposables);
            CollectionExtensions.AddTo(_viewModel.IsVisible.BindToVisible(_view.VotingHistoryContainer), disposables);
            CollectionExtensions.AddTo(_view
                    .VotingHistoryButton.SubscribeOnClick(
                        () => _viewModel.IsVisible.Value.Switch(_viewModel.Hide, _viewModel.Show)
                    ), disposables);
            CollectionExtensions.AddTo(_viewModel.IsVisible.WhereTrue().Subscribe(UpdateLabel), disposables);
        }

        private void UpdateLabel()
        {
            _view.VotingHistoryLabel.text = string.Join(
                "\n\n",
                _viewModel.VotingRounds.Select((round, index) => $"Round {index}" + $"\n{round}")
            );
        }
    }
}
