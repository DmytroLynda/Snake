using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.View.ExternalInterfaces;
using SnakeGame.View.Frames;
using System;
using System.Collections.Generic;

namespace SnakeGame.View
{
    public class Renderer : IRenderer
    {
        private IMap Map { get; }
        private IFramePreparer Preparer { get; }

        private IEnumerable<IFrameObject> LastFrame { get; set; }

        public Renderer(IMap map, IFramePreparer preparer)
        {
            Map = map ?? throw new ArgumentNullException(nameof(map));
            Preparer = preparer ?? throw new ArgumentNullException(nameof(preparer));

            LastFrame = new List<IFrameObject>();

            map.DrawMap();
        }

        public void DrawNewFrame(IGameObjects gameObjects, string title, string[] centerText)
        {
            Map.Clear(LastFrame);

            title ??= string.Empty;
            Map.DrawLineOverMap(title);

            centerText ??= Array.Empty<string>();
            Map.DrawText(centerText);

            if (gameObjects != null)
            {
                var frame = Preparer.PrepareFrame(gameObjects);

                foreach (IFrameObject frameObject in frame)
                {
                    foreach (PositivePoint point in frameObject.Location)
                    {
                        Map.DrawPointOn(point, frameObject.PresentationSymbol, frameObject.Color);
                    }
                }

                LastFrame = frame;
            }
        }
    }
}
