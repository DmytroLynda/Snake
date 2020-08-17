using SnakeGame.Controller.Resources;
using System.Collections.Generic;

namespace SnakeGame.Controller.ExternalInterfaces
{
    public interface IGameObjects
    {
        public IEnumerable<PositivePoint> HeroLocation { get; }
        public IEnumerable<PositivePoint> FoodLocation { get; }
    }
}