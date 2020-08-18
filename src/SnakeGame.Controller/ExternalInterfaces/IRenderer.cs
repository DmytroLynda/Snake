namespace SnakeGame.Controller.ExternalInterfaces
{
    public interface IRenderer
    {
        void DrawNewFrame(IGameObjects gameObjects, string title, string centerText);
    }
}
