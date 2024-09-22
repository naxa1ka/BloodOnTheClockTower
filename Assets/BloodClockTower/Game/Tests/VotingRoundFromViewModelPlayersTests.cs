// using System;
// using System.Collections.Generic;
// using System.Linq;
// using FluentAssertions;
// using NSubstitute;
// using NUnit.Framework;
// using Nxlk.Bool;
// using UniRx;
//
// namespace BloodClockTower.Game.Tests
// {
//     public class VotingRoundFromViewModelPlayersTests
//     {
//         [Test]
//         public void Constructor_WithValidPlayerViewModels_ShouldInitializePropertiesCorrectly()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true),
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Charlie", isParticipant: true),
//                 CreatePlayerViewModel("Dave")
//             };
//
//             // Act
//             var votingRound = new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             votingRound.Initiator.Should().NotBeNull();
//             votingRound.Initiator.Name.Value.Value.Should().Be("Alice");
//
//             votingRound.Nominee.Should().NotBeNull();
//             votingRound.Nominee.Name.Value.Value.Should().Be("Bob");
//
//             votingRound.Participants.Should().HaveCount(2);
//             votingRound.Participants.Select(p => p.Name.Value.Value).Should().Contain(new[] { "Alice", "Bob" });
//
//             votingRound.IgnoredParticipants.Should().HaveCount(1);
//             votingRound.IgnoredParticipants.Select(p => p.Name.Value.Value).Should().Contain("Dave");
//         }
//
//         [Test]
//         public void Constructor_WithNoInitiator_ShouldThrowInvalidOperationException()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Charlie"),
//                 CreatePlayerViewModel("Dave", isIgnored: true)
//             };
//
//             // Act
//             Action act = () => new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             act.Should().Throw<InvalidOperationException>()
//                 .WithMessage("Sequence contains no matching element");
//         }
//
//         [Test]
//         public void Constructor_WithMultipleInitiators_ShouldThrowInvalidOperationException()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true),
//                 CreatePlayerViewModel("Eve", isInitiator: true),
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Charlie")
//             };
//
//             // Act
//             Action act = () => new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             act.Should().Throw<InvalidOperationException>()
//                 .WithMessage("Sequence contains more than one matching element");
//         }
//
//         [Test]
//         public void Constructor_WithNoNominee_ShouldThrowInvalidOperationException()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true),
//                 CreatePlayerViewModel("Charlie"),
//                 CreatePlayerViewModel("Dave", isIgnored: true)
//             };
//
//             // Act
//             Action act = () => new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             act.Should().Throw<InvalidOperationException>()
//                 .WithMessage("Sequence contains no matching element");
//         }
//
//         [Test]
//         public void Constructor_WithMultipleNominees_ShouldThrowInvalidOperationException()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true),
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Eve", isNominee: true),
//                 CreatePlayerViewModel("Charlie")
//             };
//
//             // Act
//             Action act = () => new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             act.Should().Throw<InvalidOperationException>()
//                 .WithMessage("Sequence contains more than one matching element");
//         }
//
//         [Test]
//         public void Constructor_WithNoParticipants_ShouldInitializeParticipantsAsEmpty()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true),
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Dave", isIgnored: true)
//             };
//
//             // Act
//             var votingRound = new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             votingRound.Participants.Should().BeEmpty();
//         }
//
//         [Test]
//         public void Constructor_WithAllParticipants_ShouldIncludeAllParticipants()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true),
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Charlie"),
//                 CreatePlayerViewModel("Dave")
//             };
//
//             // Assume that `IsInitiator` and `IsNominee` imply participation
//             // So, Alice and Bob are participants, and Charlie and Dave are also participants by default
//
//             // Act
//             var votingRound = new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             votingRound.Participants.Should().HaveCount(4);
//             votingRound.Participants.Select(p => p.Name.Value.Value).Should().Contain(new[] { "Alice", "Bob", "Charlie", "Dave" });
//         }
//
//         [Test]
//         public void Constructor_WithNoIgnoredParticipants_ShouldInitializeIgnoredParticipantsAsEmpty()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true),
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Charlie")
//             };
//
//             // Act
//             var votingRound = new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             votingRound.IgnoredParticipants.Should().BeEmpty();
//         }
//
//         [Test]
//         public void Constructor_WithIgnoredParticipants_ShouldIncludeOnlyIgnoredParticipants()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true),
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Charlie", isIgnored: true),
//                 CreatePlayerViewModel("Dave", isIgnored: true)
//             };
//
//             // Act
//             var votingRound = new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             votingRound.IgnoredParticipants.Should().HaveCount(2);
//             votingRound.IgnoredParticipants.Select(p => p.Name.Value.Value).Should().Contain(new[] { "Charlie", "Dave" });
//         }
//
//
//         [Test]
//         public void Constructor_WithPlayerAsBothParticipantAndIgnoredParticipant_ShouldIncludeInBothCollections()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>
//             {
//                 CreatePlayerViewModel("Alice", isInitiator: true, isIgnored: true),
//                 CreatePlayerViewModel("Bob", isNominee: true),
//                 CreatePlayerViewModel("Charlie", isIgnored: true)
//             };
//
//             // Act
//             var votingRound = new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             votingRound.Participants.Should().HaveCount(2);
//             votingRound.Participants.Select(p => p.Name.Value.Value).Should().Contain(new[] { "Alice", "Bob" });
//
//             votingRound.IgnoredParticipants.Should().HaveCount(2);
//             votingRound.IgnoredParticipants.Select(p => p.Name.Value.Value).Should().Contain(new[] { "Alice", "Charlie" });
//         }
//
//         [Test]
//         public void Constructor_WithEmptyPlayerList_ShouldThrowInvalidOperationException()
//         {
//             // Arrange
//             var players = new List<PlayerViewModel>();
//
//             // Act
//             Action act = () => new VotingRoundFromViewModelPlayers(players);
//
//             // Assert
//             act.Should().Throw<InvalidOperationException>()
//                 .WithMessage("Sequence contains no matching element");
//         }
//
//         private static PlayerViewModel CreatePlayerViewModel(
//             string name,
//             bool isInitiator = false,
//             bool isNominee = false,
//             bool isParticipant = false)
//         {
//             var player = Substitute.For<IPlayer>();
//             player.Name.Returns(new ReactiveProperty<PlayerName>(PlayerName.From(name)));
//             player.IsAlive.Returns(new ReactiveProperty<bool>(true));
//
//             var playerViewModel = new PlayerViewModel(player);
//             if (isInitiator)
//                 playerViewModel.MarkInitiator();
//             if (isNominee)
//                 playerViewModel.MarkNominee();
//             if (isParticipant)
//                 playerViewModel.MarkParticipant();
//             return playerViewModel;
//         }
//     }
// }
