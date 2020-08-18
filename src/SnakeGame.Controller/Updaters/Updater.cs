using System;
using System.Runtime.CompilerServices;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Controller.Updaters.States;

[assembly: InternalsVisibleTo("SnakeGame.Controller.Tests")]

namespace SnakeGame.Controller.Updaters
{
    public class Updater : IUpdater
    {
        private ILogic GameLogic { get; }
        private IRenderer ViewRenderer { get; }
        private IUserInputListener Listener { get; }
        private GameState GameState { get; }

        private KeyType LastPressedKey { get; set; }
        
        public Updater(ILogic gameLogic, IRenderer viewRenderer, IUserInputListener listener, GameState state)
        {
            GameLogic = gameLogic ?? throw new ArgumentNullException(nameof(gameLogic));
            ViewRenderer = viewRenderer ?? throw new ArgumentNullException(nameof(viewRenderer));
            Listener = listener ?? throw new ArgumentNullException(nameof(listener));
            GameState = state ?? throw new ArgumentNullException(nameof(state));

            Listener.KeyWasPress += ProcessKeystroke;
            Listener.BeginListenInput();

            
            LastPressedKey = KeyType.Unknown;
        }

        public void Update()
        {
            GameState.Update(GameLogic, ViewRenderer, LastPressedKey);
        }

        private void ProcessKeystroke(object sender, KeyTypeEventArgs eventArgs)
        {
            #region Check for null

            if (sender is null) throw new ArgumentNullException(nameof(sender));
            if (eventArgs is null) throw new ArgumentNullException(nameof(eventArgs));

            #endregion

            LastPressedKey = eventArgs.Key;
        }
    }
}