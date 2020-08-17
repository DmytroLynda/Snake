using System.Collections.Generic;
using SnakeGame.Controller.Resources;

namespace SnakeGame.Logic.ExternalInterfaces
{
    public interface IFruitCreator
    {
        IFruit Create(IEnumerable<PositivePoint> except);
    }
}