using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnakeGame.Controller.Resources;

namespace SnakeGame.Controller.Tests
{
    [TestFixture]
    internal class PositivePointTests
    {
        #region Constructor tests

        [Test]
        [TestCase(0, -1)]
        [TestCase(-1,0)]
        [TestCase(-1,-1)]
        public void PositivePoint_NegativeParameter_ThrowsArgumentException(int x, int y)
        {
            void Act() => new PositivePoint(x, y);

            Assert.Throws<ArgumentException>(Act);
        }

        [Test]
        [TestCase(0,0)]
        [TestCase(1,0)]
        [TestCase(0,1)]
        [TestCase(1,1)]
        public void PositivePoint_PositiveParameter_DoesNotThrowAnyExceptions(int x, int y)
        {
            void Act() => new PositivePoint(x, y);

            Assert.DoesNotThrow(Act);
        }

        #endregion
    }
}
