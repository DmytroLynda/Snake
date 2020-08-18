using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.Creators;
using SnakeGame.Logic.ExternalInterfaces;

namespace SnakeGame.Logic.Tests
{
    [TestFixture]
    internal class SnakeCreatorTest
    {
        #region Constructor tests

        [Test]
        [TestCaseSource(nameof(ConstructorNullCases))]
        public void SnakeCreator_NullParameters_ThrowsArgumentNull(IMapConfiguration configuration, IMapCalculator calculator)
        {
            void Act() => new SnakeCreator(configuration, calculator);

            Assert.Throws<ArgumentNullException>(Act);
        }

        private static IEnumerable<TestCaseData> ConstructorNullCases
        {
            get
            {
                yield return new TestCaseData(null, Mock.Of<IMapCalculator>());
                yield return new TestCaseData(Mock.Of<IMapConfiguration>(), null);
            }
        }

        #endregion

        #region Create tests

        [Test]
        [TestCase(6,6,4)]
        [TestCase(5,5,4)]
        [TestCase(4,4,4)]
        [TestCase(3,3,3)]
        [TestCase(2,2,2)]
        [TestCase(1,1,1)]
        public void Create_ReturnsSnakeWithCorrectLength(int mapHeight, int mapWidth, int expectedLength)
        {
            var configuration = Mock.Of<IMapConfiguration>(conf =>
                conf.Height == mapHeight &&
                conf.Width == mapWidth);
            var calculator = Mock.Of<IMapCalculator>(calc => 
                calc.IsInMap(It.IsAny<IEnumerable<PositivePoint>>(), It.IsAny<IMapConfiguration>()));

            var snake = new SnakeCreator(configuration, calculator).Create();
            var actualLength = snake.GetLocation().Count();

            Assert.That(actualLength, Is.EqualTo(expectedLength));
        }

        #endregion
    }
}
