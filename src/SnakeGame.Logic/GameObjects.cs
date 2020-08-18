using System;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using System.Collections.Generic;
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
            FoodLocation = food ?? throw new ArgumentNullException(nameof(food));
            HeroLocation = hero ?? throw new ArgumentNullException(nameof(hero));
        }
    }
}