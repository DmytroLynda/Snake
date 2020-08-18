using SnakeGame.Controller.Resources;
using System.Collections.Generic;

namespace SnakeGame.Logic.ExternalInterfaces
{
    public interface IFruit
    {
        public IEnumerable<PositivePoint> GetLocation();
    }
}
