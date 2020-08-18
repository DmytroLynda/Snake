using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;

namespace SnakeGame.Controller.Updaters.States
{
    public interface IState
    {
        IState Update(ILogic gameLogic, IRenderer viewRenderer, KeyType lastPressedKey);
    }
}
