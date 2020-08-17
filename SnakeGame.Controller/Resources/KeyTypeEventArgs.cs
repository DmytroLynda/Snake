using System;

namespace SnakeGame.Controller.Resources
{
    public class KeyTypeEventArgs : EventArgs
    {
        public readonly KeyType Key;

        public KeyTypeEventArgs(KeyType key)
        {
            Key = key;
        }
    }
}
