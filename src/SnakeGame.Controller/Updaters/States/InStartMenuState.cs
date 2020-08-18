﻿using System.Runtime.CompilerServices;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Helpers;

[assembly: InternalsVisibleTo("SnakeGame.Controller.Tests")]

namespace SnakeGame.Controller.Updaters.States
{
    internal class InStartMenuState : IState
    {
        private bool startInfoWasRender;

        public IState Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey)
        {
            #region Checks for null
            NullHandlingHelper.ExternalCheckForNull<ILogic>(gameLogic);
            NullHandlingHelper.ExternalCheckForNull<IRenderer>(viewRenderer);
            #endregion

            if (!startInfoWasRender)
            {
                string greeting = "WELCOME TO\nTHE SNAKE GAME";
                string helpMessage = "PRESS ENTER";
                string message = $"{greeting}\n{helpMessage}";

                viewRenderer.DrawNewFrame(new EmptyGameObjects(), string.Empty, message);

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