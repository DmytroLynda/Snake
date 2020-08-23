using SnakeGame.Controller.Resources;
using System;
using System.Collections.Generic;
using SnakeGame.View.Frames;

namespace SnakeGame.View.ExternalInterfaces
{
    public interface IMap
    {
        void Clear(IEnumerable<IFrameObject> clearedFrame);
        void DrawMap();
        void DrawPointOn(PositivePoint point, char presentationSymbol, ConsoleColor color);
        void DrawLineOverMap(string line);
        void DrawText(string[] text);
        void Initialize(string title);
    }
}