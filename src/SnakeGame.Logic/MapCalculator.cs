using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.ExternalInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SnakeGame.Logic.Tests")]

namespace SnakeGame.Logic
{
    internal class MapCalculator : IMapCalculator
    {
        public PositivePoint MovePoint(PositivePoint point, IMapConfiguration map, Direction direction)
        {
            #region Check for null

            if (map is null) throw new ArgumentNullException(nameof(map));

            #endregion

            Point movedPoint = MovePoint(direction, point);

            PositivePoint result = FixGoingOutOfBounds(movedPoint, map);

            return result;
        }


        public Direction CalculateHeadDirection(PositivePoint head, PositivePoint tail)
        {
            if (head.X > tail.X && head.Y == tail.Y)
            {
                return Direction.Right;
            }
            else if (head.X < tail.X && head.Y == tail.Y)
            {
                return Direction.Left;
            }
            else if (head.X == tail.X && head.Y > tail.Y)
            {
                return Direction.Down;
            }
            else if (head.X == tail.X && head.Y < tail.Y)
            {
                return Direction.Up;
            }
            else
            {
                throw new ArgumentException("Passed head and tail don't close.");
            }
        }


        public bool IsInMap(PositivePoint point, IMapConfiguration configuration)
        {
            #region Check for null

            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            #endregion

            return point.X < configuration.Width &&
                   point.Y < configuration.Height &&
                   point.X >= 0 &&
                   point.Y >= 0;
        }

        public bool IsInMap(IEnumerable<PositivePoint> points, IMapConfiguration configuration)
        {
            #region Check for null

            if (points is null) throw new ArgumentNullException(nameof(points));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            #endregion

            return points.Any(point => IsInMap(point, configuration));
        }


        private static PositivePoint FixGoingOutOfBounds(Point processedPoint, IMapConfiguration map)
        {
            #region Check for null

            if (map is null) throw new ArgumentNullException(nameof(map));

            #endregion

            int resultX = processedPoint.X;
            int resultY = processedPoint.Y;

            if (processedPoint.X >= map.Width)
            {
                resultX = 0;
            }
            else if (processedPoint.X < 0)
            {
                resultX = map.Width - 1;
            }

            if (processedPoint.Y >= map.Height)
            {
                resultY = 0;
            }
            else if (processedPoint.Y < 0)
            {
                resultY = map.Height - 1;
            }

            return new PositivePoint(resultX, resultY);
        }

        private static Point MovePoint(Direction direction, PositivePoint point)
        {
            return direction switch
            {
                Direction.Up => new Point(point.X, point.Y - 1),
                Direction.Down => new Point(point.X, point.Y + 1),
                Direction.Left => new Point(point.X - 1, point.Y),
                Direction.Right => new Point(point.X + 1, point.Y),
                _ => throw new ArgumentException($"The {nameof(Direction)}.{direction} is not processed.")
            };
        }
    }
}
