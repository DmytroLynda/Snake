using System;
using System.Drawing;

namespace SnakeGame.Controller.Resources
{
    public readonly struct PositivePoint
    {
        private readonly Point point;

        public int X { get => point.X; }
        public int Y { get => point.Y; }

        public PositivePoint(int x, int y)
        {
            CheckForPositive(x);
            CheckForPositive(y);

            point = new Point(x, y);
        }

        public override bool Equals(object obj)
        {
            return obj is PositivePoint other && Equals(other);
        }

        public bool Equals(PositivePoint other)
        {
            return point.Equals(other.point);
        }

        public override int GetHashCode()
        {
            return point.GetHashCode();
        }

        public override string ToString()
        {
            return $"X:{point.X}, Y:{point.Y}.";
        }

        public static bool operator ==(PositivePoint left, PositivePoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PositivePoint left, PositivePoint right)
        {
            return !(left == right);
        }

        private static void CheckForPositive(int number)
        {
            if (number < 0)
            {
                var message = string.Format($"\nPassed {nameof(number)} can't be less then zero. The {nameof(number)} was {number}.");
                throw new ArgumentException(message);
            }
        }
    }
}
