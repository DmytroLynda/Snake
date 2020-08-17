using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.Creators;
using SnakeGame.Logic.ExternalInterfaces;
using SnakeGame.Logic.Exceptions;

namespace SnakeGame.Logic.Tests
{
    [TestFixture]
    public class GameLogicTests
    {
        private IFruitCreator fakeFruitCreator;
        private IFruit fakeFruit;

        private ISnakeCreator fakeSnakeCreator;
        private ISnake fakeSnake;

        private GameLogic gameLogic;

        [SetUp]
        public void SetUp()
        {
            fakeFruit = Mock.Of<IFruit>(MockBehavior.Default);

            var fakeFruitCreatorMock = new Mock<IFruitCreator>(MockBehavior.Default);
            fakeFruitCreatorMock.Setup(creator => creator.Create(It.IsAny<IEnumerable<PositivePoint>>())).Returns(fakeFruit);
            fakeFruitCreator = fakeFruitCreatorMock.Object;


            fakeSnake = Mock.Of<ISnake>(MockBehavior.Default);

            var fakeSnakeCreatorMock = new Mock<ISnakeCreator>(MockBehavior.Default);
            fakeSnakeCreatorMock.Setup(creator => creator.Create()).Returns(fakeSnake);
            fakeSnakeCreator = fakeSnakeCreatorMock.Object;

            gameLogic = new GameLogic(fakeFruitCreator, fakeSnakeCreator);
        }

        #region Constructor tests

        [Test]
        [TestCaseSource(nameof(ConstructorNullCases))]
        public void GameLogic_NullParameters_ThrowsArgumentException(ISnakeCreator snakeCreator, IFruitCreator fruitCreator)
        {
            void Act() => new GameLogic(fruitCreator, snakeCreator);

            Assert.Throws<ArgumentNullException>(Act);
        }

        private static IEnumerable<TestCaseData> ConstructorNullCases
        {
            get
            {
                yield return new TestCaseData(null, null);
                yield return new TestCaseData(null, Mock.Of<IFruitCreator>());
                yield return new TestCaseData(Mock.Of<ISnakeCreator>(), null);
            }
        }

        [Test]
        public void GameLogic_GameIsGoing()
        {
            var game = new GameLogic(fakeFruitCreator, fakeSnakeCreator);

            Assert.That(game.IsGameOver, Is.False);
        }

        [Test]
        public void GameLogic_ScoreEqualZero()
        {
            const int expectedScore = 0;

            var game = new GameLogic(fakeFruitCreator, fakeSnakeCreator);

            Assert.That(game.Score, Is.EqualTo(expectedScore));
        }

        #endregion

        #region ProcessNextGameStep

        [Test]
        [TestCase(KeyType.Up)]
        [TestCase(KeyType.Down)]
        [TestCase(KeyType.Left)]
        [TestCase(KeyType.Right)]
        [TestCase(KeyType.Enter)]
        [TestCase(KeyType.Unknown)]
        public void ProcessNextGameStep_UseCrawlStep(KeyType keyType)
        {
            gameLogic.ProcessNextGameStep(keyType);

            Mock.Get(fakeSnake).Verify(snake => snake.CrawlStep(It.IsAny<Direction>()), Times.Once);
        }

        [Test]
        public void ProcessNextGameStep_CanEatFruit_UseGrowUp()
        {
            Mock.Get(fakeSnake).Setup(snake => snake.CanEat(It.IsAny<IFruit>())).Returns(true);

            gameLogic.ProcessNextGameStep(default);

            Mock.Get(fakeSnake).Verify(snake => snake.GrowUp(), Times.Once);
        }

        [Test]
        public void ProcessNextGameStep_CanEatItself_GameIsOver()
        {
            Mock.Get(fakeSnake).Setup(snake => snake.CanEatItSelf()).Returns(true);

            gameLogic.ProcessNextGameStep(default);

            Assert.That(gameLogic.IsGameOver, Is.True);
        }

        [Test]
        public void ProcessNextGameStep_EatFruit_CreateNewFruit()
        {
            Mock.Get(fakeSnake).Setup(snake => snake.CanEat(It.IsAny<IFruit>())).Returns(true);
            Mock.Get(fakeFruitCreator).Invocations.Clear();

            gameLogic.ProcessNextGameStep(default);
            
            Mock.Get(fakeFruitCreator).Verify(creator => creator.Create(It.IsAny<IEnumerable<PositivePoint>>()), Times.Once);
        }

        [Test]
        public void ProcessNextGameStep_GameIsAlreadyOver_ThrowsGameIsOverException()
        {
            
            gameLogic.GetType().GetProperty($"{nameof(gameLogic.IsGameOver)}")
                ?.SetValue(gameLogic, true);

            void Act() => gameLogic.ProcessNextGameStep(default);

            Assert.Throws<GameIsOverException>(Act);
        }

        [Test]
        public void ProcessNextGameStep_CreatesGameObjectFromSnakeLocation()
        {
            var snakeLocation = Mock.Of<IEnumerable<PositivePoint>>(MockBehavior.Default);
            Mock.Get(fakeSnake).Setup(snake => snake.GetLocation()).Returns(snakeLocation);

            var actual = gameLogic.ProcessNextGameStep(default);

            Assert.That(actual.HeroLocation, Is.EqualTo(snakeLocation));
        }

        [Test]
        public void ProcessNextGameStep_CreatesGameObjectFromFruitLocations()
        {
            var fruitLocation = Mock.Of<IEnumerable<PositivePoint>>(MockBehavior.Default);
            Mock.Get(fakeFruit).Setup(fruit => fruit.GetLocation()).Returns(fruitLocation);

            var actual = gameLogic.ProcessNextGameStep(default);

            Assert.That(actual.FoodLocation, Is.EqualTo(fruitLocation));
        }

        [Test]
        [TestCase(KeyType.Up, Direction.Up)]
        [TestCase(KeyType.Down, Direction.Down)]
        [TestCase(KeyType.Right, Direction.Right)]
        [TestCase(KeyType.Left, Direction.Left)]
        public void ProcessNextGameStep_WithDirectionKeyType_ExecuteCrawlingWithPassedDirection(KeyType passed, Direction expected)
        {
            gameLogic.ProcessNextGameStep(passed);

            Mock.Get(fakeSnake).Verify(snake => snake.CrawlStep(expected), Times.Once);
        }

        [Test]
        [TestCase(KeyType.Enter)]
        [TestCase(KeyType.Unknown)]
        public void ProcessNextGameStep_WithNotDirectionKeyType_ExecuteCrawlingWithLastDirection(KeyType passed)
        {
            var lastDirection = KeyType.Left;
            gameLogic.ProcessNextGameStep(lastDirection);
            var expectedDirection = Direction.Left;

            gameLogic.ProcessNextGameStep(passed);

            Mock.Get(fakeSnake).Verify(snake => snake.CrawlStep(expectedDirection), Times.Exactly(2));
        }

        #endregion

        #region NewGame tests

        [Test]
        public void NewGame_ResetsScore_0()
        {
            Mock.Get(fakeSnake)
                .Setup(snake => snake.CanEat(It.IsAny<IFruit>()))
                .Returns(true);
            var initialScore = gameLogic.Score;

            gameLogic.ProcessNextGameStep(default);
            var increasedScore = gameLogic.Score;
            gameLogic.NewGame();

            Assert.That(increasedScore, Is.GreaterThan(initialScore),
                () => "Increased score is not greater than initial, it can depend from increasing after snakes eating.");
            Assert.That(gameLogic.Score, Is.Zero);
        }

        [Test]
        public void NewGame_ResetsState_IsNotOver()
        {
            Mock.Get(fakeSnake)
                .Setup(snake => snake.CanEatItSelf()).Returns(true);

            gameLogic.ProcessNextGameStep(default);
            var gameIsOver = gameLogic.IsGameOver;
            gameLogic.NewGame();

            Assert.That(gameIsOver, Is.True,
                () => "The game state is not correct, it can depend from change state after CanEatItSelf checking.");
            Assert.That(gameLogic.IsGameOver, Is.False);
        }

        [Test]
        public void NewGame_CreatesNewSnake()
        {
            Mock.Get(fakeSnakeCreator).Invocations.Clear();

            gameLogic.NewGame();

            Mock.Get(fakeSnakeCreator).Verify(creator => creator.Create(), Times.Once);
        }

        [Test]
        public void NewGame_CreatesNewFruit()
        {
            Mock.Get(fakeFruitCreator).Invocations.Clear();

            gameLogic.NewGame();

            Mock.Get(fakeFruitCreator).Verify(creator => creator.Create(It.IsAny<IEnumerable<PositivePoint>>()), Times.Once);
        }

        #endregion
    }
}
