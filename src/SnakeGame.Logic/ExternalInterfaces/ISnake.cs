using SnakeGame.Controller.Resources;
using System.Collections.Generic;

namespace SnakeGame.Logic.ExternalInterfaces
{
    public interface ISnake
    {
        bool CanEat(IFruit fruit);
        void GrowUp();
        bool CanEatItSelf();
        IEnumerable<PositivePoint> GetLocation();
        void CrawlStep(Direction direction);
    }
}
