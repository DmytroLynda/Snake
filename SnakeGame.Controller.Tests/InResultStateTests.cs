﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Controller.Updaters.States;

namespace SnakeGame.Controller.Tests
{
    [TestFixture]
    class InResultStateTests
    {
        private ILogic fakeLogic;
        private IRenderer fakeRenderer;
        private InResultState state;

        [SetUp]
        public void SetUp()
        {
            fakeLogic = Mock.Of<ILogic>();
            fakeRenderer = Mock.Of<IRenderer>();
            state = new InResultState();
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
                yield return new TestCaseData(null, Mock.Of<IRenderer>());
                yield return new TestCaseData(Mock.Of<ILogic>(), null);
            }
        }

        [Test]
        public void Update_CorrectParameters_DrawsResultMenuWithScoreFromLogic()
        {
            const int expectedScore = 10;
            Mock.Get(fakeLogic).SetupGet(logic => logic.Score).Returns(expectedScore);

            state.Update(fakeLogic, fakeRenderer, default);

            Mock.Get(fakeLogic).VerifyGet(logic => logic.Score, Times.Once);
        }

        [Test]
        [TestCase(KeyType.Up)]
        [TestCase(KeyType.Down)]
        [TestCase(KeyType.Left)]
        [TestCase(KeyType.Right)]
        [TestCase(KeyType.Unknown)]
        public void Update_NoEnter_ReturnsInResultState(KeyType noEnter)
        {
            var expected = state.GetType();

            var actual = state.Update(fakeLogic, fakeRenderer, noEnter).GetType();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Update_Enter_ReturnsInGameState()
        {
            var expected = typeof(InGameState);

            var actual = state.Update(fakeLogic, fakeRenderer, KeyType.Enter).GetType();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
