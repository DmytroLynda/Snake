using System.Collections.Generic;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;

namespace SnakeGame.Logic.ExternalInterfaces
{
    public interface IMapCalculator
    {
        PositivePoint MovePoint(PositivePoint point, IMapConfiguration map, Direction direction);
        Direction CalculateHeadDirection(PositivePoint head, PositivePoint tail);
        bool IsInMap(PositivePoint point, IMapConfiguration configuration);
        bool IsInMap(IEnumerable<PositivePoint> points, IMapConfiguration configuration);
    }
}