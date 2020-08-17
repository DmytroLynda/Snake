using SnakeGame.Controller.Resources;
using System;

namespace SnakeGame.Controller.ExternalInterfaces
{
    public interface IUserInputListener
    {
        event EventHandler<KeyTypeEventArgs> KeyWasPress;

        void BeginListenInput();
        void EndListenInput();
    }
}
