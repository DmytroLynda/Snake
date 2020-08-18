using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Helpers;

[assembly: InternalsVisibleTo("SnakeGame.Controller.Tests")]

namespace SnakeGame.Controller.Updaters.States
{
    internal class InGameState : IState
    {
        public IState Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey)
        {
            #region Check for null
            NullHandlingHelper.ExternalCheckForNull<ILogic>(gameLogic);
            NullHandlingHelper.ExternalCheckForNull<IRenderer>(viewRenderer);
            #endregion

            if (gameLogic.IsGameOver)
            {
                return new InResultState();
            }
            else
            {
                var gameObjects = gameLogic.ProcessNextGameStep(lastPressedKey);

                var scoreMessage = "Score: " + gameLogic.Score.ToString();
                viewRenderer.DrawNewFrame(gameObjects, scoreMessage, Array.Empty<string>());

                return this;
            }
        }
    }
}
