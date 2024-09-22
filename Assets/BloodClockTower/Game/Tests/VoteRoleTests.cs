using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace BloodClockTower.Game.Tests
{
    public class VoteRoleTests
    {
        public static IEnumerable<TestCaseData> BooleanCombinations
        {
            get
            {
                bool[] bools = { false, true };
                foreach (var isInitiator in bools)
                {
                    foreach (var isNominee in bools)
                    {
                        foreach (var isParticipant in bools)
                        {
                            yield return new TestCaseData(
                                isInitiator,
                                isNominee,
                                isParticipant
                            ).SetName(
                                $"Initiator:{isInitiator}, Nominee:{isNominee}, Participant:{isParticipant}"
                            );
                        }
                    }
                }
            }
        }

        [Test, TestCaseSource(nameof(BooleanCombinations))]
        public void WhenIsIgnored_ThenReturnTrueOnlyWhenAllPropertiesAreFalse(
            bool isInitiator,
            bool isNominee,
            bool isParticipant
        )
        {
            // Arrange
            var voteRole = new VoteRole(isInitiator, isNominee, isParticipant);

            // Act
            var isIgnored = voteRole.IsIgnored;

            // Assert
            isIgnored.Should().Be(!isInitiator && !isNominee && !isParticipant);
        }

        [Test, TestCaseSource(nameof(BooleanCombinations))]
        public void WhenMarkInitiator_ThenInitiatorIsTrue(
            bool isInitiator,
            bool isNominee,
            bool isParticipant
        )
        {
            // Arrange
            var voteRole = new VoteRole(isInitiator, isNominee, isParticipant);

            // Act
            var newVoteRole = voteRole.MarkInitiator;

            // Assert
            newVoteRole.IsInitiator.Should().BeTrue();
            newVoteRole.IsNominee.Should().Be(isNominee);
            newVoteRole.IsParticipant.Should().Be(isParticipant);
        }

        [Test, TestCaseSource(nameof(BooleanCombinations))]
        public void WhenUnmarkInitiator_ThenInitiatorIsFalse(
            bool isInitiator,
            bool isNominee,
            bool isParticipant
        )
        {
            // Arrange
            var voteRole = new VoteRole(isInitiator, isNominee, isParticipant);

            // Act
            var newVoteRole = voteRole.UnmarkInitiator;

            // Assert
            newVoteRole.IsInitiator.Should().BeFalse();
            newVoteRole.IsNominee.Should().Be(isNominee);
            newVoteRole.IsParticipant.Should().Be(isParticipant);
        }

        [Test, TestCaseSource(nameof(BooleanCombinations))]
        public void WhenMarkNominee_ThenNomineeIsTrue(
            bool isInitiator,
            bool isNominee,
            bool isParticipant
        )
        {
            // Arrange
            var voteRole = new VoteRole(isInitiator, isNominee, isParticipant);

            // Act
            var newVoteRole = voteRole.MarkNominee;

            // Assert
            newVoteRole.IsNominee.Should().BeTrue();
            newVoteRole.IsInitiator.Should().Be(isInitiator);
            newVoteRole.IsParticipant.Should().Be(isParticipant);
        }

        [Test, TestCaseSource(nameof(BooleanCombinations))]
        public void WhenUnmarkNominee_ThenNomineeIsFalse(
            bool isInitiator,
            bool isNominee,
            bool isParticipant
        )
        {
            // Arrange
            var voteRole = new VoteRole(isInitiator, isNominee, isParticipant);

            // Act
            var newVoteRole = voteRole.UnmarkNominee;

            // Assert
            newVoteRole.IsNominee.Should().BeFalse();
            newVoteRole.IsInitiator.Should().Be(isInitiator);
            newVoteRole.IsParticipant.Should().Be(isParticipant);
        }

        [Test, TestCaseSource(nameof(BooleanCombinations))]
        public void WhenMarkParticipant_ThenParticipantIsTrue(
            bool isInitiator,
            bool isNominee,
            bool isParticipant
        )
        {
            // Arrange
            var voteRole = new VoteRole(isInitiator, isNominee, isParticipant);

            // Act
            var newVoteRole = voteRole.MarkParticipant;

            // Assert
            newVoteRole.IsParticipant.Should().BeTrue();
            newVoteRole.IsInitiator.Should().Be(isInitiator);
            newVoteRole.IsNominee.Should().Be(isNominee);
        }

        [Test, TestCaseSource(nameof(BooleanCombinations))]
        public void WhenUnmarkParticipant_ThenParticipantIsFalse(
            bool isInitiator,
            bool isNominee,
            bool isParticipant
        )
        {
            // Arrange
            var voteRole = new VoteRole(isInitiator, isNominee, isParticipant);

            // Act
            var newVoteRole = voteRole.UnmarkParticipant;

            // Assert
            newVoteRole.IsParticipant.Should().BeFalse();
            newVoteRole.IsInitiator.Should().Be(isInitiator);
            newVoteRole.IsNominee.Should().Be(isNominee);
        }

        [Test]
        public void WhenDefault_ThenAllPropertiesAreFalse()
        {
            // Act
            var voteRole = VoteRole.Default;

            // Assert
            voteRole.IsInitiator.Should().BeFalse();
            voteRole.IsNominee.Should().BeFalse();
            voteRole.IsParticipant.Should().BeFalse();
        }
    }
}
