using SnakeGame.Controller.Resources;

namespace SnakeGame.Controller.ExternalInterfaces
{
    public interface ILogic
    {
        bool IsGameOver { get; }
        int Score { get; }

        IGameObjects ProcessNextGameStep(KeyType lastPressedKey);
        void NewGame();
    }
}
