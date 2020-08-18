using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Helpers;
using SnakeGame.Logic.ExternalInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Logic.Creators
{
    public class FruitCreator : IFruitCreator
    {
        protected readonly IMapConfiguration mapConfiguration;

        public FruitCreator(IMapConfiguration mapConfiguration)
        {
            #region Check for null

            NullHandlingHelper.ExternalCheckForNull<IMapConfiguration>(mapConfiguration);

            #endregion

            this.mapConfiguration = mapConfiguration;
        }

        public virtual IFruit Create(IEnumerable<PositivePoint> except)
        {
            except ??= Enumerable.Empty<PositivePoint>();

            Random random = new Random();
            PositivePoint randomPosition;
            do
            {
                int randomX = random.Next(0, mapConfiguration.Width - 1);
                int randomY = random.Next(0, mapConfiguration.Height - 1);

                randomPosition = new PositivePoint(randomX, randomY);
            } while (except.Contains(randomPosition));

            return new Fruit(randomPosition);
        }
    }
}
