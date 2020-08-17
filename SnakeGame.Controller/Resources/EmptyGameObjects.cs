using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeGame.Controller.ExternalInterfaces;

namespace SnakeGame.Controller.Resources
{
    internal class EmptyGameObjects : IGameObjects
    {
        public EmptyGameObjects()
        {
            HeroLocation = Enumerable.Empty<PositivePoint>();
            FoodLocation = Enumerable.Empty<PositivePoint>();
        }

        public IEnumerable<PositivePoint> HeroLocation { get; }
        public IEnumerable<PositivePoint> FoodLocation { get; }
    }
}
