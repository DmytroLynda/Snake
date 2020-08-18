using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SnakeGame.Logic.Tests")]

namespace SnakeGame.Logic
{
    internal class GameObjects : IGameObjects
    {
        public IEnumerable<PositivePoint> FoodLocation { get; }
        
        public IEnumerable<PositivePoint> HeroLocation { get; }

        public GameObjects(IEnumerable<PositivePoint> hero, IEnumerable<PositivePoint> food)
        {
            #region Checks for null
            NullHandlingHelper.ExternalCheckForNull<IEnumerable<Point>>(food);
            NullHandlingHelper.ExternalCheckForNull<IEnumerable<Point>>(hero);
            #endregion

            FoodLocation = food;
            HeroLocation = hero;
        }
    }
}