using System.Runtime.CompilerServices;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Controller.Updaters.States;
using SnakeGame.Helpers;

[assembly: InternalsVisibleTo("SnakeGame.Controller.Tests")]

namespace SnakeGame.Controller.Updaters
{
    public class Updater : IUpdater
    {
        private ILogic GameLogic { get; }
        private IRenderer ViewRenderer { get; }
        private IUserInputListener Listener { get; }

        private KeyType LastPressedKey { get; set; }
        private GameState GameState { get; }

        public Updater(ILogic gameLogic, IRenderer viewRenderer, IUserInputListener listener, GameState state)
        {
            #region Checks for null
            NullHandlingHelper.ExternalCheckForNull<ILogic>(gameLogic);
            NullHandlingHelper.ExternalCheckForNull<IRenderer>(viewRenderer);
            NullHandlingHelper.ExternalCheckForNull<IUserInputListener>(listener);
            NullHandlingHelper.ExternalCheckForNull<GameState>(state);
            #endregion

            GameLogic = gameLogic;
            ViewRenderer = viewRenderer;
            Listener = listener;

            Listener.KeyWasPress += ProcessKeystroke;
            Listener.BeginListenInput();

            GameState = state;
            LastPressedKey = KeyType.Unknown;
        }

        public void Update()
        {
            GameState.Update(GameLogic, ViewRenderer, LastPressedKey);
        }

        private void ProcessKeystroke(object sender, KeyTypeEventArgs eventArgs)
        {
            #region Check for null
            NullHandlingHelper.ExternalCheckForNull<object>(sender);
            NullHandlingHelper.ExternalCheckForNull<KeyTypeEventArgs>(eventArgs);
            #endregion

            LastPressedKey = eventArgs.Key;
        }
    }
}