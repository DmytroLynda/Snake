using SnakeGame.Controller.Resources;
using System.Collections.Generic;

namespace SnakeGame.Controller.ExternalInterfaces
{
    public interface IGameObjects
    {
        IEnumerable<PositivePoint> HeroLocation { get; }
        IEnumerable<PositivePoint> FoodLocation { get; }
    }
}