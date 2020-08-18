using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using SnakeGame.Controller.Resources;
using SnakeGame.Helpers;

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
            #region Checks for null
            NullHandlingHelper.ExternalCheckForNull<IEnumerable<Point>>(location);
            #endregion

            Location = location;
            PresentationSymbol = presentationSymbol;
            Color = color;
        }

        public FrameObject(PositivePoint location, char presentationSymbol, ConsoleColor color)
            : this(
                  new List<PositivePoint> { location },
                  presentationSymbol,
                  color)
        { }
    }
}