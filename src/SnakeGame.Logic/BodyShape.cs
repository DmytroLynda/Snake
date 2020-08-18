using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.ExternalInterfaces;

namespace SnakeGame.Logic
{
    internal class BodyShape : IBodyShape
    {
        private IList<PositivePoint> shape;
        private readonly int height;
        private readonly int width;

        public BodyShape(IEnumerable<PositivePoint> shape, int height, int width)
        {
            #region Check values

            if (height < 0) throw new ArgumentOutOfRangeException(nameof(height));
            if (width < 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (shape is null) throw new ArgumentNullException(nameof(shape));

            #endregion

            this.shape = shape.ToList();
            this.height = height;
            this.width = width;

            CheckForCorrect(shape);
        }

        public IEnumerable<PositivePoint> Shape
        {
            get => shape;
            set
            {
                #region Check for null

                if (shape is null) throw new ArgumentNullException(nameof(value));

                #endregion

                CheckForCorrect(value);

                shape = value.ToList();
            }
        }

        public IEnumerable<PositivePoint> GetExceptHead()
        {
            return shape.Skip(1);
        }

        public PositivePoint GetHead()
        {
            const int headIndex = 0;
            return shape[headIndex];
        }

        public void Insert(int index, PositivePoint point)
        {
            #region Check value

            if (index <= 0) throw new ArgumentOutOfRangeException(nameof(index));

            #endregion

            shape.Insert(index, point);

            CheckForCorrect(shape);
        }
        
        private void CheckForCorrect(IEnumerable<PositivePoint> shape)
        {
            #region Check for null

            Debug.Assert(shape != null, nameof(this.shape) + " != null");

            #endregion

            var positions = shape.ToList();

            if (!positions.Any())
            {
                throw new ArgumentException($"{nameof(shape)} can't be empty.");
            }

            const int maxNumberOfPermissibleDifferences = 1;
            for (int i = 0; i < positions.Count - 1; i++)
            {
                int differenceCounter = 0;
                PositivePoint firstPartPosition = positions[i];
                PositivePoint lastPartPosition = positions[i + 1];

                differenceCounter += Math.Abs(firstPartPosition.X - lastPartPosition.X) % (width - 1);
                differenceCounter += Math.Abs(firstPartPosition.Y - lastPartPosition.Y) % (height - 1);

                if (differenceCounter > maxNumberOfPermissibleDifferences)
                {
                    throw new ArgumentException("Passed shape of body was incorrect.");
                }
            }
        }
    }
}