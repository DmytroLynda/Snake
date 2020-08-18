using System;
using System.Collections.Generic;
using NUnit.Framework;
using SnakeGame.Controller.Resources;

namespace SnakeGame.Logic.Tests
{
    [TestFixture]
    internal class BodyShapeTests
    {
        private readonly int height = 4;
        private readonly int width = 4;

        [Test]
        [TestCaseSource(nameof(CorrectShapeCases))]

        #region Constructor tests

        public void BodyShape_CreateWithCorrectShape_CreatesObject(IEnumerable<PositivePoint> shape)
        {
            void Act() => new BodyShape(shape, height, width);

            Assert.DoesNotThrow(Act);
        }

        private static IEnumerable<TestCaseData> CorrectShapeCases
        {
            get
            {
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(0,0),
                    new PositivePoint(0,1),
                    new PositivePoint(0,2),
                });
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(0,0),
                    new PositivePoint(1,0),
                    new PositivePoint(2,0),
                });
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(0,0),
                    new PositivePoint(1,0),
                    new PositivePoint(1,1),
                });
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(0,0),
                    new PositivePoint(0,1),
                    new PositivePoint(1,1),
                });
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(0,0),
                    new PositivePoint(0,0),
                });
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(0,0),
                    new PositivePoint(3,0),
                });
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(0,0),
                    new PositivePoint(0,3),
                });
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(3,0),
                    new PositivePoint(0,0),
                });
                yield return new TestCaseData(new List<PositivePoint>
                {
                    new PositivePoint(0,3),
                    new PositivePoint(0,0),
                });
            }
        }

        [Test]
        public void BodyShape_CreateWithNull_ThrowsArgumentNullException()
        {
            void Act() => new BodyShape(null, height, width);

            Assert.Throws<ArgumentNullException>(Act);
        }

        [Test]
        [TestCaseSource(nameof(IncorrectShapeCases))]
        public void BodyShape_CreateWithIncorrectShape_ThrowsArgumentException(IEnumerable<PositivePoint> shape)
        {
            void Act() => new BodyShape(shape, height, width);

            Assert.Throws<ArgumentException>(Act);
        }



        private static IEnumerable<TestCaseData> IncorrectShapeCases()
        {
            yield return new TestCaseData(new List<PositivePoint>
            {
                new PositivePoint(0,0),
                new PositivePoint(1,1)
            });
            yield return new TestCaseData(new List<PositivePoint>
            {
                new PositivePoint(0,0),
                new PositivePoint(2,0)
            });
            yield return new TestCaseData(new List<PositivePoint>
            {
                new PositivePoint(0,0),
                new PositivePoint(0,2)
            });
        }

        [Test]
        public void BodyShape_CreateWithEmptyShape_ThrowsArgumentException()
        {
            var emptyList = new List<PositivePoint>();

            void Act() => new BodyShape(emptyList, height, width);

            Assert.Throws<ArgumentException>(Act);
        }

        #endregion
    }
}
