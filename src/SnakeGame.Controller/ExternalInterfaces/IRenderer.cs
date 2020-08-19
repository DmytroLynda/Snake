using System.Threading.Tasks;

namespace SnakeGame.Controller.ExternalInterfaces
{
    public interface IRenderer
    {
        void DrawNewFrame(IGameObjects gameObjects, string title, string[] centerText);
        Task DrawNewFrameAsync(IGameObjects gameObjects, string title, string[] centerText);
    }
}
