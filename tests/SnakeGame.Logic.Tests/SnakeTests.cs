using Moq;
using NUnit.Framework;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.ExternalInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Logic.Tests
{
    [TestFixture]
    internal class SnakeTests
    {
        private IMapConfiguration fakeConfiguration;
        private IMapCalculator fakeCalculator;
        private IBodyShape fakeShape;

        private Snake defaultSnake;
        private Direction firstSnakeDirection;


        [OneTimeSetUp]
        public void Initialize()
        {
            var configurationMock = new Mock<IMapConfiguration>(MockBehavior.Default);
            configurationMock.SetupGet(conf => conf.Height).Returns(3);
            configurationMock.SetupGet(conf => conf.Width).Returns(3);
            fakeConfiguration = configurationMock.Object;

            firstSnakeDirection = Direction.Left;
        }

        [SetUp]
        public void SetUp()
        {
            var calculatorMock = new Mock<IMapCalculator>(MockBehavior.Default);

            calculatorMock.Setup(calc =>
                    calc.IsInMap(It.IsAny<PositivePoint>(), It.IsAny<IMapConfiguration>())).Returns(true);

            calculatorMock.Setup(calc =>
                    calc.IsInMap(It.IsAny<IEnumerable<PositivePoint>>(), It.IsAny<IMapConfiguration>())).Returns(true);

            calculatorMock.Setup(x => x.CalculateHeadDirection(It.IsAny<PositivePoint>(), It.IsAny<PositivePoint>()))
                .Returns(firstSnakeDirection);

            fakeCalculator = calculatorMock.Object;

            var startingLocation = new List<PositivePoint>
            {
                new PositivePoint(0, 0),
                new PositivePoint(1,0),
                new PositivePoint(2,0)
            };

            var shapeMock = new Mock<IBodyShape>(MockBehavior.Default);
            shapeMock.SetupGet(shape => shape.Shape).Returns(startingLocation);
            shapeMock.Setup(shape => shape.GetHead()).Returns(startingLocation.First);
            shapeMock.Setup(shape => shape.GetExceptHead()).Returns(startingLocation.Skip(1).ToList());
            fakeShape = shapeMock.Object;
            defaultSnake = new Snake(fakeConfiguration, fakeCalculator, fakeShape);
        }

        #region Counstuctor tests
        [Test]
        [TestCaseSource(nameof(ConstructorValuesCases))]
        public void Snake_NullValues_ThrowsException(
            IMapConfiguration configuration,
            IMapCalculator calculator,
            IBodyShape startBodyShape)
        {
            void Act() => new Snake(configuration, calculator, startBodyShape);

            Assert.Throws<ArgumentNullException>(Act);
        }

        public static IEnumerable<TestCaseData> ConstructorValuesCases
        {
            get
            {
                yield return new TestCaseData(
                    null,
                    null,
                    null);

                yield return new TestCaseData(
                    null,
                    Mock.Of<IMapCalculator>(conf => conf.IsInMap(It.IsAny<PositivePoint>(), It.IsAny<IMapConfiguration>()) == true),
                    Mock.Of<IBodyShape>());

                yield return new TestCaseData(
                    Mock.Of<IMapConfiguration>(),
                    null,
                    Mock.Of<IBodyShape>());

                yield return new TestCaseData(
                    Mock.Of<IMapConfiguration>(),
                    Mock.Of<IMapCalculator>(conf => conf.IsInMap(It.IsAny<PositivePoint>(), It.IsAny<IMapConfiguration>()) == true),
                    null);
            }
        }


        [Test]
        [TestCase(3, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 3)]
        [TestCase(4, 4)]
        public void Snake_LocationNotInMap_ThrowException(int xLocation, int yLocation)
        {
            Mock.Get(fakeCalculator)
                .Setup(x => x.IsInMap(It.IsAny<IEnumerable<PositivePoint>>(), It.IsAny<IMapConfiguration>()))
                .Returns(false);

            Mock.Get(fakeShape).SetupGet(shape => shape.Shape).Returns(new List<PositivePoint>{
                new PositivePoint(xLocation, yLocation),
            });


            void Act() => new Snake(fakeConfiguration, fakeCalculator, fakeShape);

            Assert.Throws<ArgumentException>(Act);
        }

        [Test]
        public void Snake_CalculateHeadDirection()
        {
            Mock.Get(fakeCalculator).Verify(x => x.CalculateHeadDirection(It.IsAny<PositivePoint>(), It.IsAny<PositivePoint>()), Times.Once());
        }

        [Test]
        public void Snake_CheckIfIsInMap()
        {
            Mock.Get(fakeCalculator).Verify(x => x.IsInMap(It.IsAny<IEnumerable<PositivePoint>>(), It.IsAny<IMapConfiguration>()), Times.Once());
        }
        #endregion

        #region CanEat tests
        [Test]
        [TestCase(0, 0, true)]
        [TestCase(0, 1, false)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, false)]
        [TestCase(2, 2, false)]
        [TestCase(2, 1, false)]
        [TestCase(1, 2, false)]
        public void CanEat_Fruits_ReturnsTrueIfFruitIsOnHead(int fruitX, int fruitY, bool expected)
        {
            var fruit = Mock.Of<IFruit>(x => x.GetLocation() == new List<PositivePoint>
                {
                    new PositivePoint(fruitX, fruitY)
                });

            bool actual = defaultSnake.CanEat(fruit);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanEat_NullFruit_ReturnFalse()
        {
            bool actual = defaultSnake.CanEat(null);

            Assert.That(actual, Is.False);
        }
        #endregion

        #region CanEatItSelf tests
        [Test]
        public void CanEatItself_BodyIsOnHead_ReturnsTrue()
        {
            Mock.Get(fakeShape).Setup(shape => shape.GetHead()).Returns(new PositivePoint(0, 0));
            Mock.Get(fakeShape).Setup(shape => shape.GetExceptHead())
                .Returns(new List<PositivePoint> { new PositivePoint(0, 0) });

            var snakeWithTailOnHead = new Snake(fakeConfiguration, fakeCalculator, fakeShape);

            bool actual = snakeWithTailOnHead.CanEatItSelf();

            Assert.That(actual, Is.True);
        }

        [Test]
        public void CanEatItself_BodyIsNotOnHead_ReturnsFalse()
        {
            Mock.Get(fakeShape).SetupGet(shape => shape.Shape).Returns(new List<PositivePoint>
            {
                new PositivePoint(1, 1), //Head
                new PositivePoint(0 ,1),
                new PositivePoint(1, 0),
                new PositivePoint(0, 0),
                new PositivePoint(2, 1),
                new PositivePoint(1, 2),
                new PositivePoint(2, 2)
            });
            var snakeWithCorrectBody = new Snake(fakeConfiguration, fakeCalculator, fakeShape);

            bool actual = snakeWithCorrectBody.CanEatItSelf();

            Assert.That(actual, Is.False);
        }
        #endregion

        #region GrowUp
        [Test]
        public void GrowUp_InsertPoint()
        {
            Mock.Get(fakeShape).Setup(shape => shape.Insert(It.IsAny<int>(), It.IsAny<PositivePoint>())).Verifiable();

            defaultSnake.GrowUp();

            Mock.Get(fakeShape).Verify(shape => shape.Insert(It.IsAny<int>(), It.IsAny<PositivePoint>()), Times.Once);
        }

        [Test]
        public void GrowUp_FruitSwallowedByHead()
        {
            defaultSnake.GrowUp();

            Mock.Get(fakeShape).Verify(shape => shape.Insert(1, defaultSnake.GetLocation().First()), Times.Once);
        }
        #endregion

        #region CrawlStep
        [Test]
        [TestCase(Direction.Up)]
        [TestCase(Direction.Down)]
        [TestCase(Direction.Left)]
        [TestCase(Direction.Right)]
        public void CrawlStep_SnakesBodyGoesBehindHead(Direction direction)
        {
            int lengthWithoutHead = defaultSnake.GetLocation().Count() - 1;
            var expectedSnakesBodyAfterSteep = defaultSnake.GetLocation().Take(lengthWithoutHead).ToList();
            var doesNotMeterHeadPosition = It.IsAny<PositivePoint>();
            expectedSnakesBodyAfterSteep.Insert(0, doesNotMeterHeadPosition);

            defaultSnake.CrawlStep(direction);

            Mock.Get(fakeShape).VerifySet(shape => shape.Shape = expectedSnakesBodyAfterSteep, Times.Once);
        }

        [Test]
        public void CrawlStep_SnakeDoesNotRespondOnDirectionThatPointsInBody()
        {
            const Direction inBodyDirection = Direction.Right;

            defaultSnake.CrawlStep(inBodyDirection);
            var actualDirection = defaultSnake.LastDirection;

            Assert.That(actualDirection, Is.EqualTo(firstSnakeDirection));
        }

        [Test]
        public void CrawlStep_UseMovePoint()
        {
            defaultSnake.CrawlStep(default);

            Mock.Get(fakeCalculator).Verify(
                x => x.MovePoint(It.IsAny<PositivePoint>(),
                It.IsAny<IMapConfiguration>(), It.IsAny<Direction>()),
                Times.Once);
        }
        #endregion
    }
}