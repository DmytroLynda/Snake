using SnakeGame.Controller.Resources;
using SnakeGame.Logic.ExternalInterfaces;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SnakeGame.Logic.Tests")]

namespace SnakeGame.Logic
{
    internal class Fruit : IFruit
    {
        private readonly PositivePoint location;

        public Fruit(PositivePoint location)
        {
            this.location = location;
        }

        public IEnumerable<PositivePoint> GetLocation()
        {
            return new List<PositivePoint> {
                location
            };
        }
    }
}
