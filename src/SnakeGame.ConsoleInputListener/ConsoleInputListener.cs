using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame.Input
{
    public class ConsoleInputListener : IUserInputListener
    {
        public event EventHandler<KeyTypeEventArgs> KeyWasPress;

        private bool IsWorking { get; set; }

        public ConsoleInputListener()
        {
            IsWorking = false;

            InitializeListenAsync();
        }

        public void BeginListenInput()
        {
            IsWorking = true;
        }

        public void EndListenInput()
        {
            IsWorking = false;
        }
        private async Task InitializeListenAsync()
        {
            await Task.Run(Listen);
        }

        private void Listen()
        {
            Thread.CurrentThread.Name = "ConsoleListener";

            KeyType lastKey = default;
            while (true)
            {
                if (IsWorking)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    KeyType pressedKey = GetKeyType(key);

                    if (pressedKey != KeyType.Unknown && pressedKey != lastKey)
                    {
                        KeyTypeEventArgs eventArgs = new KeyTypeEventArgs(pressedKey);
                        KeyWasPress?.Invoke(this, eventArgs);
                    }

                    lastKey = pressedKey;
                }

                const int sleepTime = 100;
                Thread.Sleep(sleepTime);
            }
        }

        private static KeyType GetKeyType(ConsoleKeyInfo key)
        {
            ConsoleKey workKey = key.Key;
            return workKey switch
            {
                ConsoleKey.Enter => KeyType.Enter,
                ConsoleKey.UpArrow => KeyType.Up,
                ConsoleKey.DownArrow => KeyType.Down,
                ConsoleKey.LeftArrow => KeyType.Left,
                ConsoleKey.RightArrow => KeyType.Right,
                _ => KeyType.Unknown,
            };
        }
    }
}
