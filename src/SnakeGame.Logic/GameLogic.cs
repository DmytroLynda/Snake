using SnakeGame.Controller.ExternalInterfaces;
using SnakeGame.Controller.Resources;
using SnakeGame.Logic.Exceptions;
using SnakeGame.Logic.ExternalInterfaces;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SnakeGame")]

namespace SnakeGame.Logic
{
    public class GameLogic : ILogic
    {
        private readonly IFruitCreator fruitCreator;
        private readonly ISnakeCreator snakeCreator;

        public bool IsGameOver { get; private set; }
        public int Score { get; private set; }

        private ISnake Snake { get; set; } 
        private IFruit Fruit { get; set; }
        private Direction LastDirection { get; set; }

        public GameLogic(IFruitCreator fruitCreator, ISnakeCreator snakeCreator)
        {
            this.fruitCreator = fruitCreator ?? throw new ArgumentNullException(nameof(fruitCreator));
            this.snakeCreator = snakeCreator ?? throw new ArgumentNullException(nameof(snakeCreator));

            NewGame();
        }

        public IGameObjects ProcessNextGameStep(KeyType keyType)
        {
            #region Check game state
            if (IsGameOver)
            {
                string message = string.Format($"\nIssue: The game is already over and the next step can't be calculate." +
                                               $"\nSolution: before execution of the method check the {nameof(IsGameOver)} property." +
                                               $"\nValue: {nameof(IsGameOver)}: {IsGameOver}.");
                throw new GameIsOverException(message);
            }
            #endregion

            Direction direction = GetDirection(keyType);

            Snake.CrawlStep(direction);

            if (Snake.CanEat(Fruit))
            {
                Snake.GrowUp();
                Fruit = fruitCreator.Create(Snake.GetLocation());
                Score++;
            }
            else if (Snake.CanEatItSelf())
            {
                IsGameOver = true;
            }

            IGameObjects gameObjects = new GameObjects(Snake.GetLocation(), Fruit.GetLocation());

            return gameObjects;
        }

        public void NewGame()
        {
            IsGameOver = false;
            Score = 0;

            Snake = snakeCreator.Create();
            Fruit = fruitCreator.Create(Snake.GetLocation());
        }

        private Direction GetDirection(KeyType keyType)
        {
            Direction direction;
            if (IsDirection(keyType))
            {
                direction = KeyTypeToDirection(keyType);
                LastDirection = direction;
            }
            else
            {
                direction = LastDirection;
            }

            return direction;
        }

        private static bool IsDirection(KeyType keyType)
        {
            return keyType switch
            {
                KeyType.Up => true,
                KeyType.Down => true,
                KeyType.Left => true,
                KeyType.Right => true,

                KeyType.Enter => false,
                KeyType.Unknown => false,
                _ => throw new ArgumentException($"The {nameof(KeyType)}.{keyType} is not processed."),
            };
        }

        private static Direction KeyTypeToDirection(KeyType keyType)
        {
            return keyType switch
            {
                KeyType.Up => Direction.Up,
                KeyType.Down => Direction.Down,
                KeyType.Left => Direction.Left,
                KeyType.Right => Direction.Right,
                _ => throw new ArgumentException($"The {nameof(KeyType)}.{keyType} is not processed.")
            };
        }
    }
}