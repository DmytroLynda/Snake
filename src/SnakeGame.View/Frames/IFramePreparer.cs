using System.Collections.Generic;
using SnakeGame.Controller.ExternalInterfaces;

namespace SnakeGame.View.Frames
{
    public interface IFramePreparer
    {
        IEnumerable<IFrameObject> PrepareFrame(IGameObjects gameObjects);
    }
}
