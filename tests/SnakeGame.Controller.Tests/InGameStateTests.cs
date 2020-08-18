using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Controller.Updaters.States;

namespace SnakeGame.Controller.Tests
{
    [TestFixture]
    internal class InGameStateTests
    {
        private ILogic fakeLogic;
        private IRenderer fakeRenderer;
        private InGameState state;

        [SetUp]
        public void SetUp()
        {
            fakeLogic = Mock.Of<ILogic>();
            fakeRenderer = Mock.Of<IRenderer>();
            state = new InGameState();
        }

        #region Constructor tests

        [Test]
        [TestCaseSource(nameof(UpdateNullCases))]
        public void InGameState_UpdateWithNullParameters_ThrowsArgumentNull(ILogic logic, IRenderer renderer)
        {
            var state = new InGameState();

            void Act() => state.Update(logic, renderer, default);

            Assert.Throws<ArgumentNullException>(Act);
        }

        private static IEnumerable<TestCaseData> UpdateNullCases
        {
            get
            {
                yield return new TestCaseData(null, Mock.Of<IRenderer>());
                yield return new TestCaseData(Mock.Of<ILogic>(), null);
            }
        }

        #endregion

        #region Update tests

        [Test]
        public void Update_GameIsNotOver_ReturnsCurrentState()
        {
            Mock.Get(fakeLogic).Setup(logic => logic.IsGameOver).Returns(false);
            var expected = typeof(InGameState);

            var actual = state.Update(fakeLogic, fakeRenderer, default).GetType();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Update_GameIsOver_ReturnsInResultState()
        {
            Mock.Get(fakeLogic).Setup(logic => logic.IsGameOver).Returns(true);
            var expected = typeof(InResultState);

            var actual = state.Update(fakeLogic, fakeRenderer, default).GetType();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(KeyType.Up)]
        [TestCase(KeyType.Down)]
        [TestCase(KeyType.Left)]
        [TestCase(KeyType.Right)]
        [TestCase(KeyType.Enter)]
        [TestCase(KeyType.Unknown)]
        public void Update_PassKey_ProcessNextGameStepWithTheKey(KeyType key)
        {
            Mock.Get(fakeLogic).Setup(logic => logic.IsGameOver).Returns(false);

            state.Update(fakeLogic, fakeRenderer, key);

            Mock.Get(fakeLogic).Verify(logic => logic.ProcessNextGameStep(key), Times.Once);
        }

        [Test]
        public void Update_ShouldDrawFrameFromProcessedGameObjects()
        {
            var expectedGameObjects = Mock.Of<IGameObjects>();
            Mock.Get(fakeLogic).Setup(logic => logic.ProcessNextGameStep(It.IsAny<KeyType>()))
                .Returns(expectedGameObjects);

            state.Update(fakeLogic, fakeRenderer, default);

            Mock.Get(fakeRenderer).Verify(renderer =>
                renderer.DrawNewFrame(
                    expectedGameObjects,
                    It.IsAny<string>(),
                    It.IsAny<string[]>()));
        }

        [Test]
        public void Update_ShouldDrawFrameWithScoreFromLogic()
        {
            var expectedScore = 10;
            Mock.Get(fakeLogic).SetupGet(logic => logic.Score)
                .Returns(expectedScore);

            state.Update(fakeLogic, fakeRenderer, default);

            Mock.Get(fakeLogic).VerifyGet(logic => logic.Score, Times.Once);
        }

        #endregion
    }
}
