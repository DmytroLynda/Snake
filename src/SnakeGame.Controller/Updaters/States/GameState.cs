using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Helpers;

namespace SnakeGame.Controller.Updaters.States
{
    public class GameState
    {
        private IState State { get; set; }

        public GameState(IState beginState)
        {
            #region Check for null

            NullHandlingHelper.ExternalCheckForNull<IState>(beginState);

            #endregion

            State = beginState;
        }
        public virtual void Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey)
        {
            #region Checks for null
            NullHandlingHelper.ExternalCheckForNull<ILogic>(gameLogic);
            NullHandlingHelper.ExternalCheckForNull<IRenderer>(viewRenderer);
            #endregion

            State = State.Update(gameLogic, viewRenderer, lastPressedKey);
        }
    }
}
