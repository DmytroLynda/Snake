using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SnakeGame.Logic.Tests")]

namespace SnakeGame.Logic.Exceptions
{
    internal class GameIsOverException : Exception
    {
        public GameIsOverException(string message) : base(message)
        {
        }
    }
}
