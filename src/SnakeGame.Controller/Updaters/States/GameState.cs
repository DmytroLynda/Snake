using System;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;

namespace SnakeGame.Controller.Updaters.States
{
    public class GameState
    {
        private IState State { get; set; }

        public GameState(IState beginState)
        {
            State = beginState ?? throw new ArgumentNullException(nameof(beginState));
        }
        public virtual void Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey)
        {
            #region Checks for null

            if (gameLogic is null) throw new ArgumentNullException(nameof(gameLogic));
            if (viewRenderer is null) throw new ArgumentNullException(nameof(viewRenderer));

            #endregion

            State = State.Update(gameLogic, viewRenderer, lastPressedKey);
        }
    }
}
