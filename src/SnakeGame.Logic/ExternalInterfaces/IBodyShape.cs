using System.Collections.Generic;
using SnakeGame.Controller.Resources;

namespace SnakeGame.Logic.ExternalInterfaces
{
    public interface IBodyShape
    {
        IEnumerable<PositivePoint> Shape { get; set; }
        IEnumerable<PositivePoint> GetExceptHead();
        PositivePoint GetHead();
        void Insert(int index, PositivePoint point);
    }
}