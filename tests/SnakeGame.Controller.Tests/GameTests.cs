using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SnakeGame.Controller.ExternalInterfaces;
using Range = Moq.Range;

namespace SnakeGame.Controller.Tests
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void Game_NullUpdater_ThrowsArgumentNull()
        {
            IUpdater updater = null;

            void Act() => new Game(updater);

            Assert.Throws<ArgumentNullException>(Act);
        }

        [Test]
        public void Start_UpdatesGameEvery_50_Milliseconds()
        {
            const int delay = 50;
            const int executeTimes = 10;
            var updater = Mock.Of<IUpdater>();
            var game = new Game(updater);

            var task = game.StartAsync(delay);

            task.Wait(delay * executeTimes);
            Mock.Get(updater).Verify(
                updater => updater.Update(), 
                Times.Between(executeTimes - 1, executeTimes + 1, Range.Inclusive));
        }
    }
}
