using Moq;
using NUnit.Framework;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.View.ExternalInterfaces;
using SnakeGame.View.Frames;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.View.Tests
{
    public class RendererTests
    {
        private Renderer renderer;
        private IMap fakeMap;
        private IFramePreparer fakePreparer;

        [SetUp]
        public void Setup()
        {
            fakeMap = Mock.Of<IMap>();
            fakePreparer = Mock.Of<IFramePreparer>();

            renderer = new Renderer(fakeMap, fakePreparer);
        }

        #region Constructor tests

        [Test]
        [TestCaseSource(nameof(ConstructorNullCases))]
        public void Renderer_NullParameters_ThrowsArgumentNull(IMap map, IFramePreparer preparer)
        {
            void Act() => new Renderer(map, preparer);

            Assert.Throws<ArgumentNullException>(Act);
        }

        private static IEnumerable<TestCaseData> ConstructorNullCases
        {
            get
            {
                yield return new TestCaseData(null, Mock.Of<IFramePreparer>());
                yield return new TestCaseData(Mock.Of<IMap>(), null);
            }
        }

        [Test]
        public void Renderer_AlwaysDrawsMap()
        {
            Mock.Get(fakeMap).Invocations.Clear();

            var unused = new Renderer(fakeMap, fakePreparer);

            Mock.Get(fakeMap).Verify(map => map.DrawMap(), Times.Once);
        }

        #endregion

        #region DrawNewFrame tests

        [Test]
        public void DrawNewFrame_AlwaysClearOldFrame()
        {
            renderer.DrawNewFrame(It.IsAny<IGameObjects>(), It.IsAny<string>(), It.IsAny<string>());

            Mock.Get(fakeMap).Verify(map => map.Clear(It.IsAny<IEnumerable<IFrameObject>>()), Times.Once);
        }

        [Test]
        public void DrawNewFrame_ClearsTheLastFrame()
        {
            var lastFrame = Mock.Of<IEnumerable<IFrameObject>>(frame => frame.GetEnumerator() == Enumerable.Empty<IFrameObject>());
            Mock.Get(fakePreparer)
                .Setup(preparer => preparer.PrepareFrame(It.IsAny<IGameObjects>()))
                .Returns(lastFrame);
            renderer.DrawNewFrame(Mock.Of<IGameObjects>(), It.IsAny<string>(), It.IsAny<string>());

            renderer.DrawNewFrame(It.IsAny<IGameObjects>(), It.IsAny<string>(), It.IsAny<string>());

            Mock.Get(fakeMap).Verify(map => map.Clear(lastFrame), Times.Once);
        }

        [Test]
        public void DrawNewFrame_NullGameObjects_DoesNotDrawObjects()
        {
            renderer.DrawNewFrame(null, It.IsAny<string>(), It.IsAny<string>());

            Mock.Get(fakeMap).Verify(map =>
                    map.DrawPointOn(It.IsAny<PositivePoint>(), It.IsAny<char>(), It.IsAny<ConsoleColor>()),
                Times.Never);
        }

        [Test]
        public void DrawNewFrame_DrawsAllPointsOnMap()
        {
            //Arrange
            var heroLocation = new List<PositivePoint>
            {
                new PositivePoint(0,0),
                new PositivePoint(1,0),
                new PositivePoint(2,0),
                new PositivePoint(2,1),
                new PositivePoint(2,2),
            };
            var foodLocation = new List<PositivePoint>
            {
                new PositivePoint(5,5)
            };

            var frameObjects = new List<IFrameObject>
            {
                Mock.Of<IFrameObject>(f => f.Location == heroLocation),
                Mock.Of<IFrameObject>(f => f.Location == foodLocation)
            };

            Mock.Get(fakePreparer)
                .Setup(preparer => preparer.PrepareFrame(It.IsAny<IGameObjects>()))
                .Returns(frameObjects);

            //Act
            renderer.DrawNewFrame(Mock.Of<IGameObjects>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Mock.Get(fakeMap).Verify(map =>
                map.DrawPointOn(
                    It.IsIn(heroLocation.Concat(foodLocation)),
                    It.IsAny<char>(),
                    It.IsAny<ConsoleColor>()));
        }

        [Test]
        public void DrawNewFrame_DrawsWithSpecifiedColor()
        {
            var expectedColor = ConsoleColor.Yellow;
            Mock.Get(fakePreparer)
                .Setup(preparer => preparer.PrepareFrame(It.IsAny<IGameObjects>()))
                .Returns(new List<IFrameObject>
                {
                    Mock.Of<IFrameObject>(frameObject =>
                        frameObject.Color == expectedColor && 
                        frameObject.Location == new List<PositivePoint>{It.IsAny<PositivePoint>()})
                });

            renderer.DrawNewFrame(Mock.Of<IGameObjects>(), It.IsAny<string>(), It.IsAny<string>());

            Mock.Get(fakeMap).Verify(map => 
                    map.DrawPointOn(
                        It.IsAny<PositivePoint>(),
                        It.IsAny<char>(),
                        expectedColor),
                Times.Once);
        }

        [Test]
        public void DrawNewFrame_DrawsWithSpecifiedSymbol()
        {
            var expectedSymbol = '*';
            Mock.Get(fakePreparer)
                .Setup(preparer => preparer.PrepareFrame(It.IsAny<IGameObjects>()))
                .Returns(new List<IFrameObject>
                {
                    Mock.Of<IFrameObject>(frameObject =>
                        frameObject.PresentationSymbol == expectedSymbol && 
                        frameObject.Location == new List<PositivePoint>{It.IsAny<PositivePoint>()})
                });

            renderer.DrawNewFrame(Mock.Of<IGameObjects>(), It.IsAny<string>(), It.IsAny<string>());

            Mock.Get(fakeMap).Verify(map => 
                    map.DrawPointOn(
                        It.IsAny<PositivePoint>(),
                        expectedSymbol,
                        It.IsAny<ConsoleColor>()),
                Times.Once);
        }


        [Test]
        [TestCase(null, "")]
        [TestCase("SomeValue", "SomeValue")]
        public void DrawNewFrame_PassTitle_DrawsTitle(string passedTitle, string expectedTitle)
        {
            renderer.DrawNewFrame(It.IsAny<IGameObjects>(), passedTitle, It.IsAny<string>());

            Mock.Get(fakeMap).Verify(map => map.DrawLineOverMap(expectedTitle), Times.Once);
        }

        [Test]
        [TestCase(null, "")]
        [TestCase("SomeLine", "SomeLine")]
        public void DrawNewFrame_PassLine_DrawsLine(string passedLine, string expectedLine)
        {
            renderer.DrawNewFrame(It.IsAny<IGameObjects>(), It.IsAny<string>(), passedLine);

            Mock.Get(fakeMap).Verify(map => map.DrawText(expectedLine), Times.Once);
        }

        #endregion
    }
}