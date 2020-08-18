using SnakeGame.Controller.ExternalInterfaces;

namespace SnakeGame.Controller.Resources
{
    public class MapConfiguration : IMapConfiguration
    {
        public int Height { get; }
        public int Width { get; }

        public MapConfiguration(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}
