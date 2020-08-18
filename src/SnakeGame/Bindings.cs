using System;
using Ninject.Modules;
using SnakeGame.Controller;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Controller.Updaters;
using SnakeGame.Controller.Updaters.States;
using SnakeGame.Input;
using SnakeGame.Logic;
using SnakeGame.Logic.Creators;
using SnakeGame.Logic.ExternalInterfaces;
using SnakeGame.View;
using SnakeGame.View.ExternalInterfaces;
using SnakeGame.View.Frames;

namespace SnakeGame
{
    internal class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapConfiguration>()
                .To<MapConfiguration>()
                .WithConstructorArgument("height", 11)
                .WithConstructorArgument("width", 20);
            Bind<IMapCalculator>().To<MapCalculator>();
            Bind<IFruitCreator>().To<FruitCreator>();
            Bind<ISnakeCreator>().To<SnakeCreator>();
            Bind<ILogic>().To<GameLogic>();

            Bind<IMap>()
                .To<Map>()
                .WithConstructorArgument("borderColor", ConsoleColor.White)
                .WithConstructorArgument("borderSymbol", '#');
            Bind<IFramePreparer>().To<FramePreparer>();
            Bind<IRenderer>().To<Renderer>();

            Bind<IUserInputListener>().To<ConsoleInputListener>();

            Bind<IState>()
                .ToMethod(context => new BeginStateCreator().Create());
            Bind<GameState>().To<GameState>();

            Bind<IUpdater>().To<Updater>();

            Bind<Game>().To<Game>();
        }
    }
}
