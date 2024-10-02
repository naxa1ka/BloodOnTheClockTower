using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace BloodClockTower.Game.Tests
{
    public class GameTests
    {
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _game = new Game(GamePlayersAmount.From(1), Substitute.For<IChangeNightCommand>());
        }

        [Test]
        public void WhenPreviousNight_AndNoNights_ThenThrowException()
        {
            // Act.
            Action act = () => _game.PreviousNight();

            // Assert.
            act.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void WhenNextNight_AndNoNights_ThenThrowException()
        {
            // Act.
            Action act = () => _game.NextNight();

            // Assert.
            act.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void WhenIsFirstNight_AndNoNights_ThenReturnTrue()
        {
            // Act.
            var isFirstNight = _game.IsFirstNight();

            // Assert.
            isFirstNight.Should().BeTrue();
        }

        [Test]
        public void WhenIsLastNight_AndNoNights_ThenReturnTrue()
        {
            // Act.
            var isLastNight = _game.IsLastNight();

            // Assert.
            isLastNight.Should().BeTrue();
        }

        [Test]
        public void WhenStartNewNight_AndNoNights_ThenCurrentNightIsSecondNight()
        {
            // Arrange.
            _game.StartNewNight();

            // Assert.
            _game.CurrentNight.Value.Number.Should().Be(2);
        }

        [Test]
        public void WhenPreviousNight_AndTwoNights_ThenCurrentNightIsFirstNight()
        {
            // Arrange.
            _game.StartNewNight();

            // Act.
            _game.PreviousNight();

            // Assert.
            _game.CurrentNight.Value.Number.Should().Be(1);
        }

        [Test]
        public void WhenIsFirstNight_AndTwoNights_ThenReturnFalse()
        {
            // Arrange.
            _game.StartNewNight();

            // Act.
            var isFirstNight = _game.IsFirstNight();

            // Assert.
            isFirstNight.Should().BeFalse();
        }

        [Test]
        public void WhenIsLastNight_AndTwoNights_ThenReturnTrue()
        {
            // Arrange.
            _game.StartNewNight();

            // Act.
            var isLastNight = _game.IsLastNight();

            // Assert.
            isLastNight.Should().BeTrue();
        }
    }
}
