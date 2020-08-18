using System;
using SnakeGame.Controller;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Input;
using SnakeGame.Logic;
using SnakeGame.Logic.Creators;
using SnakeGame.View;
using SnakeGame.View.ExternalInterfaces;
using System.Diagnostics;
using SnakeGame.Controller.Updaters;
using SnakeGame.Controller.Updaters.States;
using SnakeGame.View.Frames;

namespace SnakeGame
{
    public static class Program
    {
        private static void Main()
        {
            Console.CursorVisible = false;
            Trace.Listeners.Add(new DefaultTraceListener());

            IMapConfiguration mapConfig = new MapParameters(11, 20);

            ILogic logic = new GameLogic(new FruitCreator(mapConfig), new SnakeCreator(mapConfig, new MapCalculator()));

            IMap map = new Map(mapConfig, ConsoleColor.White, '#'){};
            
            IFramePreparer preparer = new FramePreparer();
            IRenderer renderer = new Renderer(map, preparer);

            IUserInputListener listener = new ConsoleInputListener();

            var state = new GameState(new BeginStateCreator().Create());

            IUpdater updater = new Updater(logic, renderer, listener, state);

            Game game = new Game(updater);
            //try
            {
                const int delayBetweenDelay = 200;
                game.Start(delayBetweenDelay);
            }
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message + e.StackTrace);

            //    throw;
            //}
        }
    }
}
