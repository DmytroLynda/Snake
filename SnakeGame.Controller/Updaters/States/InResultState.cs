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
                string finalScore = "FINAL RESULT: " + gameLogic.Score;
                string finalMessage = "THE GAME IS OVER!";
                string additionalMessage = "PRESS ENTER\nTO START AGAIN";

                string message = $"{finalMessage}\n{additionalMessage}";

                viewRenderer.DrawNewFrame(new EmptyGameObjects(), finalScore, message);

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
