using System;
using System.Runtime.CompilerServices;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Helpers;

[assembly: InternalsVisibleTo("SnakeGame.Controller.Tests")]

namespace SnakeGame.Controller.Updaters.States
{
    internal class InResultState : IState
    {
        private bool ResultWasRender { get; set; }

        public IState Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey)
        {
            #region Checks for null
            NullHandlingHelper.ExternalCheckForNull<ILogic>(gameLogic);
            NullHandlingHelper.ExternalCheckForNull<IRenderer>(viewRenderer);
            #endregion

            if (!ResultWasRender)
            {
                string resultScore = gameLogic.Score.ToString();

                string[] message =
                {
                    "FINAL RESULT: " + resultScore,
                    "THE GAME IS OVER!",
                    "PRESS ENTER",
                    "TO START AGAIN"
                };

                viewRenderer.DrawNewFrame(new EmptyGameObjects(), resultScore, message);

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
