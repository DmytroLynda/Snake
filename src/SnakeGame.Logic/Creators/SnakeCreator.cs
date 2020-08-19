using System;
using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.ExternalInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Logic.Creators
{
    public class SnakeCreator : ISnakeCreator
    {
        protected readonly IMapConfiguration MapConfiguration;
        protected readonly IMapCalculator MapCalculator;

        public SnakeCreator(IMapConfiguration mapConfiguration, IMapCalculator mapCalculator)
        {
            this.MapConfiguration = mapConfiguration ?? throw new ArgumentNullException(nameof(mapConfiguration));
            this.MapCalculator = mapCalculator ?? throw new ArgumentNullException(nameof(mapCalculator));
        }

        /// <summary>
        /// Creates snake with length of 4 or shorter if the Map configuration is shorter.
        /// </summary>
        /// <returns>Returns a created ISnake instance with a head at (0,0) and a body on the right with a maximum of 4 points.</returns>
        public virtual ISnake Create()
        {
            var location = new List<PositivePoint>();

            var firstPosition = new PositivePoint(0, 0);
            location.Add(firstPosition);

            const int maxSnakeLength = 4;
            int snakeLength = maxSnakeLength > MapConfiguration.Width ? MapConfiguration.Width : maxSnakeLength;
            for (int count = 0; count < snakeLength - 1; count++)
            {
                int nextX = location.Last().X + 1;
                int nextY = location.Last().Y;
                PositivePoint nextPosition = new PositivePoint(nextX, nextY);

                location.Add(nextPosition);
            }

            var shape = new BodyShape(location, MapConfiguration.Height, MapConfiguration.Width);

            return new Snake(MapConfiguration, MapCalculator, shape);
        }
    }
}
