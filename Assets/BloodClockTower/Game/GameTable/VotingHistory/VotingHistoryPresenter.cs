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
            _view.OpenVotingHistoryButton.Hide();
            Observable
                .CombineLatest(
                    _viewModel.VotingRounds.ObserveCountChangedWithCount(),
                    _votingSystemViewModel.CurrentState,
                    (votingRoundsCount, state) =>
                        votingRoundsCount > 0 && state == VotingSystemViewModel.State.Idle
                )
                .BindToVisible(_view.OpenVotingHistoryButton)
                .AddTo(disposables);
            _viewModel.IsVisible.BindToVisible(_view.VotingHistoryContainer).AddTo(disposables);
            _view
                .OpenVotingHistoryButton.SubscribeOnClick(() => _viewModel.Show())
                .AddTo(disposables);
            _view
                .CloseVotingHistoryButton.SubscribeOnClick(() => _viewModel.Hide())
                .AddTo(disposables);
            _viewModel.IsVisible.WhereTrue().Subscribe(UpdateLabel).AddTo(disposables);
        }

        private void UpdateLabel()
        {
            _view.VotingHistoryLabel.text = new VotingRoundsPerNight(_viewModel.VotingRounds).ToString();
        }
    }
}
