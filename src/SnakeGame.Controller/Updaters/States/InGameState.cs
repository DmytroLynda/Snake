using System;
using System.Runtime.CompilerServices;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;

[assembly: InternalsVisibleTo("SnakeGame.Controller.Tests")]

namespace SnakeGame.Controller.Updaters.States
{
    internal class InGameState : IState
    {
        public IState Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey)
        {
            #region Check for null

            if (gameLogic is null) throw new ArgumentNullException(nameof(gameLogic));
            if (viewRenderer is null) throw new ArgumentNullException(nameof(viewRenderer));

            #endregion

            if (gameLogic.IsGameOver)
            {
                return new InResultState();
            }
            else
            {
                var gameObjects = gameLogic.ProcessNextGameStep(lastPressedKey);

                var scoreMessage = "Score: " + gameLogic.Score.ToString();
                viewRenderer.DrawNewFrameAsync(gameObjects, scoreMessage, Array.Empty<string>());

                return this;
            }
        }
    }
}
