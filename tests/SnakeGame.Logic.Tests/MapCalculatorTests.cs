using Moq;
using NUnit.Framework;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using System;
using System.Collections.Generic;

namespace SnakeGame.Logic.Tests
{
    [TestFixture]
    public class MapCalculatorTests
    {
        private IMapConfiguration mapConfiguragion;
        private PositivePoint pointOnCenter;
        private MapCalculator mapCalculator;

        [OneTimeSetUp]
        public void Initialize()
        {
            mapConfiguragion = Mock.Of<IMapConfiguration>(conf => 
                conf.Height == 3 &&
                conf.Width == 3);

            pointOnCenter = new PositivePoint(1, 1);
            mapCalculator = new MapCalculator();
        }


        #region MovePoint tests

        [Test]
        [TestCaseSource(nameof(MoveCasesSource))]
        public void MovePoint_ReturnsMovedPoint(Direction direction, PositivePoint expectedPoint)
        {
            PositivePoint result = mapCalculator.MovePoint(pointOnCenter, mapConfiguragion, direction);

            TestContext.WriteLine($"Result = {result}, Expected = {expectedPoint}");
            Assert.That(result, Is.EqualTo(expectedPoint));
        }

        private static IEnumerable<TestCaseData> MoveCasesSource
        {
            get
            {
                yield return new TestCaseData(Direction.Up, new PositivePoint(1, 0));
                yield return new TestCaseData(Direction.Down, new PositivePoint(1, 2));
                yield return new TestCaseData(Direction.Left, new PositivePoint(0, 1));
                yield return new TestCaseData(Direction.Right, new PositivePoint(2, 1));
            }
        }

        [Test]
        [TestCaseSource(nameof(MoveThroughBorderCasesSource))]
        public void MovePoint_ThroughBorder_ReturnsPointOnTheOppositeSide(Direction direction, PositivePoint expectedPoint)
        {
            PositivePoint stepToBorder = mapCalculator.MovePoint(pointOnCenter, mapConfiguragion, direction);
            PositivePoint afterPassingThroughBorder = mapCalculator.MovePoint(stepToBorder, mapConfiguragion, direction);

            Assert.That(afterPassingThroughBorder, Is.EqualTo(expectedPoint));
        }
        private static IEnumerable<TestCaseData> MoveThroughBorderCasesSource
        {
            get
            {
                yield return new TestCaseData(Direction.Up, new PositivePoint(1, 2));
                yield return new TestCaseData(Direction.Down, new PositivePoint(1, 0));
                yield return new TestCaseData(Direction.Left, new PositivePoint(2, 1));
                yield return new TestCaseData(Direction.Right, new PositivePoint(0, 1));
            }
        }

        [Test]
        public void MovePoint_PassNullMap_ThrowsException()
        {
            void Act() => mapCalculator.MovePoint(point: default, map: null, direction: default);

            Assert.Throws<ArgumentNullException>(Act);
        }

        #endregion

        #region CalculateHeadDirection tests

        [Test]
        [TestCase(1, 2, Direction.Up)]
        [TestCase(2, 1, Direction.Left)]
        [TestCase(1, 0, Direction.Down)]
        [TestCase(0, 1, Direction.Right)]
        public void CalculateHeadDirection_ReturnsTrueValue(int xTail, int yTail, Direction expectedDirection)
        {
            var head = new PositivePoint(1, 1);
            var tail = new PositivePoint(xTail, yTail);

            Direction actualDirection = mapCalculator.CalculateHeadDirection(head, tail);

            Assert.That(actualDirection, Is.EqualTo(expectedDirection));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(0, 2)]
        [TestCase(2, 0)]
        [TestCase(3, 3)]
        [TestCase(3, 0)]
        [TestCase(0, 3)]
        public void CalculateHeadDirection_PassBadTail_ThrowsException(int xTail, int yTail)
        {
            var head = new PositivePoint(1, 1);
            var tail = new PositivePoint(xTail, yTail);

            void Act() => mapCalculator.CalculateHeadDirection(head, tail);

            Assert.Throws<ArgumentException>(Act);
        }

        #endregion

        #region IsInMap tests

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(0, 2)]
        [TestCase(2, 0)]
        [TestCase(2, 2)]
        [TestCase(2, 1)]
        [TestCase(1, 2)]
        public void IsInMap_PointInMap_ReturnsTrue(int xPoint, int yPoint)
        {
            var point = new PositivePoint(xPoint, yPoint);

            var actual = mapCalculator.IsInMap(point, mapConfiguragion);

            Assert.That(actual, Is.True);
        }

        [Test]
        [TestCase(3, 3)]
        [TestCase(2, 3)]
        [TestCase(3, 2)]
        [TestCase(4, 4)]
        [TestCase(1, 4)]
        [TestCase(4, 1)]
        public void IsInMap_PointIsOutsideOfMap_ReturnsFalse(int xPoint, int yPoint)
        {
            var point = new PositivePoint(xPoint, yPoint);

            var actual = mapCalculator.IsInMap(point, mapConfiguragion);

            Assert.That(actual, Is.False);
        }

        #endregion
    }
}