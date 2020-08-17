using System;
using System.Collections.Generic;
using SnakeGame.Controller.Resources;

namespace SnakeGame.View.Frames
{
    public interface IFrameObject
    {
        ConsoleColor Color { get; set; }
        IEnumerable<PositivePoint> Location { get; }
        char PresentationSymbol { get; }
    }
}