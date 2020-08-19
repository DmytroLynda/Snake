using System;
using System.Runtime.CompilerServices;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;

[assembly: InternalsVisibleTo("SnakeGame.Controller.Tests")]

namespace SnakeGame.Controller.Updaters.States
{
    internal class InStartMenuState : IState
    {
        private bool startInfoWasRender;

        public IState Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey)
        {
            #region Checks for null

            if (gameLogic is null) throw new ArgumentNullException(nameof(gameLogic));
            if (viewRenderer is null) throw new ArgumentNullException(nameof(viewRenderer));

            #endregion

            if (!startInfoWasRender)
            {
                string[] message =
                {
                    "WELCOME TO",
                    "THE SNAKE GAME",
                    "PRESS ENTER"
                };

                viewRenderer.DrawNewFrameAsync(new EmptyGameObjects(), string.Empty, message);

                startInfoWasRender = true;
            }

            if (lastPressedKey == KeyType.Enter)
            {
                gameLogic.NewGame();
                return new InGameState();
            }
            else
            {
                return this;
            }
        }
    }
}
