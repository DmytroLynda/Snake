using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SnakeGame.Controller.Resources;

[assembly: InternalsVisibleTo("SnakeGame")]

namespace SnakeGame.View.Frames
{
    internal class FrameObject : IFrameObject
    {
        public IEnumerable<PositivePoint> Location { get; }
        public char PresentationSymbol { get; }
        public ConsoleColor Color { get; set; }

        public FrameObject(IEnumerable<PositivePoint> location, char presentationSymbol, ConsoleColor color)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location));
            PresentationSymbol = presentationSymbol;
            Color = color;
        }
    }
}