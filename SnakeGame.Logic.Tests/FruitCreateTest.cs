using Moq;
using NUnit.Framework;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.Creators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Logic.Tests
{
    [TestFixture]
    public class FruitCreateTest
    {
        private const int MapWidth = 5;
        private const int MapHeight = 5;
        private FruitCreator creator;

        [SetUp]
        public void SetUp()
        {
            var configuration = Mock.Of<IMapConfiguration>(conf =>
                conf.Height == MapHeight &&
                conf.Width == MapWidth);

            creator = new FruitCreator(configuration);
        }

        [Test]
        public void FruitCreator_NullMapConfiguration_ThrowsArgumentNullException()
        {
            IMapConfiguration configuration = null;

            void Act() => new FruitCreator(configuration);

            Assert.Throws<ArgumentNullException>(Act);
        }

        [Test]
        [Repeat(100)]
        public void Create_ReturnsFruitOnMapArea()
        {
            var actualFruit = creator.Create(Enumerable.Empty<PositivePoint>()).GetLocation();

            Assert.IsFalse(actualFruit
                .Any(point =>
                    point.X >= MapWidth ||
                    point.Y >= MapHeight));
        }

        [Test]
        public void Create_ReturnsFruitNotOnPassedPositions(
            [Random(0, 4, 20, Distinct = false)] int x,
            [Random(0, 4, 20, Distinct = false)] int y)
        {
            var except = new List<PositivePoint> { new PositivePoint(x, y) };

            var actualFruitLocation = creator.Create(except).GetLocation();

            CollectionAssert.DoesNotContain(actualFruitLocation, except);
        }
    }
}
