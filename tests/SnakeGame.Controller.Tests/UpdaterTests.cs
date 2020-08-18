using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Controller.Updaters;
using SnakeGame.Controller.Updaters.States;

namespace SnakeGame.Controller.Tests
{
    [TestFixture]
    internal class UpdaterTests
    {
        private ILogic fakeLogic;
        private IRenderer fakeRenderer;
        private IUserInputListener fakeListener;
        private GameState fakeState;
        private Updater updater;

        [SetUp]
        public void SetUp()
        {
            fakeLogic = Mock.Of<ILogic>();
            fakeRenderer = Mock.Of<IRenderer>();
            fakeListener = Mock.Of<IUserInputListener>();
            fakeState = new Mock<GameState>(Mock.Of<IState>()).Object;

            updater = new Updater(fakeLogic, fakeRenderer, fakeListener, fakeState);
        }

        #region Constructor tests

        [Test]
        [TestCaseSource(nameof(ConstructorNullCases))]
        public void Updater_NullParameters_ThrowsArgumentNullException(ILogic logic, IRenderer renderer, IUserInputListener listener, GameState state)
        {
            void Act() => new Updater(logic, renderer, listener, state);

            Assert.Throws<ArgumentNullException>(Act);
        }

        private static IEnumerable<TestCaseData> ConstructorNullCases
        {
            get
            {
                yield return new TestCaseData(
                    null,
                    Mock.Of<IRenderer>(),
                    Mock.Of<IUserInputListener>(),
                    new Mock<GameState>(Mock.Of<IState>()).Object);

                yield return new TestCaseData(
                    Mock.Of<ILogic>(),
                    null,
                    Mock.Of<IUserInputListener>(),
                    new Mock<GameState>(Mock.Of<IState>()).Object);

                yield return new TestCaseData(
                    Mock.Of<ILogic>(),
                    Mock.Of<IRenderer>(),
                    null,
                    new Mock<GameState>(Mock.Of<IState>()).Object);

                yield return new TestCaseData(
                    Mock.Of<ILogic>(),
                    Mock.Of<IRenderer>(),
                    Mock.Of<IUserInputListener>(),
                    null);
            }
        }

        [Test]
        public void Updater_CorrectParameters_DoesNotThrowException()
        {
            void Act() => new Updater(fakeLogic, fakeRenderer, fakeListener, fakeState);

            Assert.DoesNotThrow(Act);
        }

        [Test]
        public void Updater_SubscribeOnListener()
        {
            Mock.Get(fakeListener).SetupAdd(listener => listener.KeyWasPress += It.IsAny<EventHandler<KeyTypeEventArgs>>());

            var unused = new Updater(fakeLogic, fakeRenderer, fakeListener, fakeState);

            Mock.Get(fakeListener).VerifyAdd(listener => listener.KeyWasPress += It.IsAny<EventHandler<KeyTypeEventArgs>>(), Times.Once);
        }

        [Test]
        public void Updater_BeginListening()
        {
            Mock.Get(fakeListener).Verify(listener => listener.BeginListenInput(), Times.Once);
        }

        [Test]
        public void Updater_SetsLastDirectionLikeUnknown()
        {
            updater.Update();

            Mock.Get(fakeState).Verify(state => state.Update(
                    It.IsAny<ILogic>(),
                    It.IsAny<IRenderer>(),
                    KeyType.Unknown),
                Times.Once);
        }

        #endregion

        #region Update tests

        [Test]
        public void Update_UseSpecifiedLogic()
        {
            updater.Update();

            Mock.Get(fakeState).Verify(state => state.Update(fakeLogic, It.IsAny<IRenderer>(), It.IsAny<KeyType>()), Times.Once);
        }

        [Test]
        public void Update_UseSpecifiedRenderer()
        {
            updater.Update();

            Mock.Get(fakeState).Verify(state => state.Update(It.IsAny<ILogic>(), fakeRenderer, It.IsAny<KeyType>()), Times.Once);
        }

        #endregion

        #region KeyWasPressed - tests behaviour after invoke the event.

        [Test]
        [TestCaseSource(nameof(EventArgsNullCases))]
        public void KeyWasPressed_WithNullEventArgs_ThrowsArgumentNull(object sender, KeyTypeEventArgs eventArgs)
        {
            void Act() => Mock.Get(fakeListener).Raise(listener => listener.KeyWasPress += null, sender, eventArgs);

            Assert.Throws<ArgumentNullException>(Act);
        }

        private static IEnumerable<TestCaseData> EventArgsNullCases
        {
            get
            {
                yield return new TestCaseData(null, new Mock<KeyTypeEventArgs>(default(KeyType)).Object);
                yield return new TestCaseData(new object(), null);
            }
        }

        [Test]
        [TestCase(KeyType.Up)]
        [TestCase(KeyType.Down)]
        [TestCase(KeyType.Left)]
        [TestCase(KeyType.Right)]
        [TestCase(KeyType.Enter)]
        [TestCase(KeyType.Unknown)]
        public void KeyWasPressed_ShouldSaveNewKey(KeyType key)
        {
            var eventArgsMock = new Mock<KeyTypeEventArgs>(key);

            Mock.Get(fakeListener).Raise(listener => listener.KeyWasPress += null, eventArgsMock.Object);
            updater.Update(); //Force updater use actual KeyType for update state.
            
            Mock.Get(fakeState).Verify(state => state.Update(
                    It.IsAny<ILogic>(),
                    It.IsAny<IRenderer>(),
                    key),
                Times.Once);
        }

        #endregion
    }
}
