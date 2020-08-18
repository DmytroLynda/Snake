using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.ExternalInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("SnakeGame.Logic.Tests")]

namespace SnakeGame.Logic
{
    internal class Snake : ISnake
    {
        private readonly IMapCalculator mapCalculator;
        private readonly IMapConfiguration mapConfiguration;
        private readonly IBodyShape bodyShape;

        public Direction LastDirection { get; private set; }

        public Snake(IMapConfiguration mapConfiguration, IMapCalculator mapCalculator, IBodyShape initialBodyShape)
        {
            #region Check values and for null

            if (mapConfiguration is null) throw new ArgumentNullException(nameof(mapConfiguration));
            if (mapCalculator is null) throw new ArgumentNullException(nameof(mapCalculator));
            if (initialBodyShape is null) throw new ArgumentNullException(nameof(initialBodyShape));

            if (!mapCalculator.IsInMap(initialBodyShape.Shape, mapConfiguration))
            {
                var message = new StringBuilder(string.Format(
                    $"\nPassed {nameof(initialBodyShape)} is outside the {nameof(mapConfiguration)}." +
                    $"\n{nameof(mapConfiguration)} was Width:{mapConfiguration.Width}, Height:{mapConfiguration.Height}."));
                foreach (var point in initialBodyShape.Shape)
                    message.Append($"\nX:{point.X}, Y:{point.Y}.");

                throw new ArgumentException(message.ToString());
            }

            #endregion

            this.mapConfiguration = mapConfiguration;
            this.mapCalculator = mapCalculator;
            this.bodyShape = initialBodyShape;

            var head = bodyShape.Shape.FirstOrDefault();
            var firstBodyPart = bodyShape.Shape.Skip(1).FirstOrDefault();
            LastDirection = this.mapCalculator.CalculateHeadDirection(head, firstBodyPart);
        }

        public bool CanEat(IFruit fruit)
        {
            if (fruit is null) return false;

            var snakeHead = bodyShape.GetHead();

            var intersect = fruit.GetLocation().Contains(snakeHead);

            return intersect;
        }

        public bool CanEatItSelf()
        {
            var snakeHead = bodyShape.GetHead();

            bool canEatItSelf = bodyShape.GetExceptHead().Contains(snakeHead);

            return canEatItSelf;
        }

        public IEnumerable<PositivePoint> GetLocation()
        {
            return bodyShape.Shape;
        }

        public void GrowUp()
        {
            var swallowFruit = bodyShape.GetHead();

            bodyShape.Insert(1, swallowFruit);
        }

        public void CrawlStep(Direction direction)
        {
            if (DoesChangeMovementInBody(direction))
            {
                MoveSnakeLocation(LastDirection);
            }
            else
            {
                MoveSnakeLocation(direction);
            }
        }

        private void MoveSnakeLocation(Direction direction)
        {
            var movedHead = mapCalculator.MovePoint(bodyShape.GetHead(), mapConfiguration, direction);
            var movedHeadList = new List<PositivePoint>(1) { movedHead };
            var movedBody = bodyShape.Shape.SkipLast(1).ToList();

            var newLocation = movedHeadList.Concat(movedBody);

            bodyShape.Shape = newLocation;

            LastDirection = direction;
        }

        private bool DoesChangeMovementInBody(Direction direction)
        {
            return direction switch
            {
                Direction.Up => LastDirection == Direction.Down,
                Direction.Down => LastDirection == Direction.Up,
                Direction.Left => LastDirection == Direction.Right,
                Direction.Right => LastDirection == Direction.Left,
                _ => throw new ArgumentException($"The {nameof(KeyType)}.{direction} is not processed.")
            };
        }
    }
}