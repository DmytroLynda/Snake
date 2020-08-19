using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.View.ExternalInterfaces;
using SnakeGame.View.Frames;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnakeGame.View
{
    public class ConsoleRenderer : IRenderer
    {
        private IMap Map { get; }
        private IFramePreparer Preparer { get; }

        private IEnumerable<IFrameObject> LastFrame { get; set; }

        public ConsoleRenderer(IMap map, IFramePreparer preparer)
        {
            Map = map ?? throw new ArgumentNullException(nameof(map));
            Preparer = preparer ?? throw new ArgumentNullException(nameof(preparer));

            LastFrame = new List<IFrameObject>();

            map.DrawMap();
        }

        public Task DrawNewFrameAsync(IGameObjects gameObjects, string title, string[] centerText)
        {
            return Task.Run(() => DrawNewFrame(gameObjects, title, centerText));
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
