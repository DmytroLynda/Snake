using Ninject;
using SnakeGame.Controller;
using SnakeGame.View.ExternalInterfaces;

namespace SnakeGame
{
    public static class Program
    {
        private static Game _game;
        

        private static void Initialize()
        {
            IKernel kernel = new StandardKernel(new Bindings());
            _game = kernel.Get<Game>();

            var map = kernel.Get<IMap>();

            const string gameName = "Snake";
            map.Initialize(gameName);
        }

        private static void Start()
        {
            const int delayBetweenDelay = 200;
            _game.Start(delayBetweenDelay);
        }

        public static void Main()
        {
            Initialize();
            Start();
        }
    }
}
