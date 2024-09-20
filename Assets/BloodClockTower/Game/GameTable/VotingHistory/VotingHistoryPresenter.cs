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
            var roundsAsString = string.Join(
                "\n\n",
                _viewModel.VotingRounds.Select(
                    (round, index) => $"Round {index}" + $"\n{FormatRound(round)}"
                )
            );
            var ignoredParticipantsOverall = _viewModel
                .VotingRounds.SelectMany(x => x.IgnoredParticipants)
                .Distinct();
            var participantsOverall = _viewModel
                .VotingRounds.SelectMany(x => x.Participants)
                .Distinct();
            _view.VotingHistoryLabel.text =
                $"{roundsAsString}\n\n{FormatIgnoredParticipantsOverall()}\\n\\n{FormatParticipantsOverall()}";

            return;
            string FormatIgnoredParticipantsOverall() =>
                $"Ignored: {string.Join(", ", ignoredParticipantsOverall)}";

            string FormatParticipantsOverall() =>
                $"Voted: {string.Join(", ", participantsOverall)}";

            string FormatRound(IVotingRound round)
            {
                return $"{round.Initiator} -> {round.Nominee}"
                    + $"\nVoted: {string.Join(", ", round.Participants)}";
            }
        }
    }
}
