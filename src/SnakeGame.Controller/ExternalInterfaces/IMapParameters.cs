using SnakeGame.Controller.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Controller.ExternalInterfaces
{
    public interface IMapConfiguration
    {
        int Width { get; }
        int Height { get; }
    }
}
