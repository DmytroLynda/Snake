using System;
using System.Collections.Generic;
using SnakeGame.Controller.ExternalInterfaces;

namespace SnakeGame.View.Frames
{
    public class FramePreparer : IFramePreparer
    {
        public ConsoleColor HeroColor { get; set; }
        public char HeroSymbol { get; set; }

        public ConsoleColor FoodColor { get; set; }
        public char FoodSymbol { get; set; }

        public FramePreparer()
        {
            HeroColor = ConsoleColor.Red;
            HeroSymbol = '*';

            FoodColor = ConsoleColor.Yellow;
            FoodSymbol = '+';
        }

        public IEnumerable<IFrameObject> PrepareFrame(IGameObjects gameObjects)
        {
            #region Check for null

            if (gameObjects is null) throw new ArgumentNullException(nameof(gameObjects));

            #endregion

            yield return new FrameObject(gameObjects.HeroLocation, HeroSymbol, HeroColor);
            yield return new FrameObject(gameObjects.FoodLocation, FoodSymbol, FoodColor);
        }
    }
}
