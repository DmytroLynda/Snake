using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Controller.Updaters.States;

namespace SnakeGame.Controller.Tests
{
    [TestFixture]
    class InStartMenuStateTests
    {
        private ILogic fakeLogic;
        private IRenderer fakeRenderer;
        private InStartMenuState state;

        [SetUp]
        public void SetUp()
        {
            state = new InStartMenuState();

            fakeLogic = Mock.Of<ILogic>();
            fakeRenderer = Mock.Of<IRenderer>();
        }

        [Test]
        [TestCaseSource(nameof(UpdateNullCases))]
        public void Update_NullParameters_ThrowsArgumentNull(ILogic logic, IRenderer renderer)
        {
            void Act() => state.Update(logic, renderer, default);

            Assert.Throws<ArgumentNullException>(Act);
        }

        private static IEnumerable<TestCaseData> UpdateNullCases
        {
            get
            {
                yield return new TestCaseData(
                    null,
                    Mock.Of<IRenderer>());

                yield return new TestCaseData(
                    Mock.Of<ILogic>(),
                    null);
            }
        }

        [Test]
        [TestCase(KeyType.Up)]
        [TestCase(KeyType.Down)]
        [TestCase(KeyType.Left)]
        [TestCase(KeyType.Right)]
        [TestCase(KeyType.Unknown)]
        public void Update_NoEnter_ReturnsInStartMenuState(KeyType key)
        {
            var expected = state.GetType();

            var actual = state.Update(fakeLogic, fakeRenderer, key).GetType();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Update_Enter_ReturnsInGameState()
        {
            var expected = typeof(InGameState);

            var actual = state.Update(fakeLogic, fakeRenderer, KeyType.Enter).GetType();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Update_Enter_InitializeNewGame()
        {
            var lastKey = KeyType.Enter;

            state.Update(fakeLogic, fakeRenderer, lastKey);

            Mock.Get(fakeLogic).Verify(logic => logic.NewGame(), Times.Once);
        }
    }
}
