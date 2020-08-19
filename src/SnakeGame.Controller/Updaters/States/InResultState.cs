using System;
using System.Runtime.CompilerServices;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;

[assembly: InternalsVisibleTo("SnakeGame.Controller.Tests")]

namespace SnakeGame.Controller.Updaters.States
{
    internal class InResultState : IState
    {
        private bool ResultWasRender { get; set; }

        public IState Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey)
        {
            #region Checks for null

            if (gameLogic is null) throw new ArgumentNullException(nameof(gameLogic));
            if (viewRenderer is null) throw new ArgumentNullException(nameof(viewRenderer));

            #endregion

            if (!ResultWasRender)
            {
                string resultScore = "FINAL RESULT: " + gameLogic.Score;

                string[] message =
                {
                    "THE GAME IS OVER!",
                    "PRESS ENTER",
                    "TO START AGAIN"
                };

                viewRenderer.DrawNewFrameAsync(new EmptyGameObjects(), resultScore, message);

                ResultWasRender = true;
            }

            bool enterWasPress = lastPressedKey == KeyType.Enter;
            if (enterWasPress)
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
