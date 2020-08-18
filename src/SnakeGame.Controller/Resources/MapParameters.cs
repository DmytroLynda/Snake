using System.Collections.Generic;
using System.Linq;
using SnakeGame.Controller.ExternalInterfaces;

namespace SnakeGame.Controller.Resources
{
    internal class MapParameters : IMapConfiguration
    {
        public int Height { get; }
        public int Width { get; }

        public MapParameters(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}
