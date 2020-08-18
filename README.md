# Snake game
Snake game is an old simple game about the snake that want to eat and grow. Will you help it?

## How to play?
For playing in the game you should [download](https://github.com/DmytroLynda/Snake/raw/master/SnakeGame.zip) the game,
or clone the repository and start the solution from the `SnakeGame` project.

## Project goals and results
The goal of this project was creating the whole application from the beginning to the end. I wanted to apply the principles of creating clean code, achieving modularity of components and covering them with tests. The result after designing the application was the practical ability to create small applications on the .Net platform, using the principles of OOP, S.O.L.I.D., CI, DRY and some design patterns. Almost the whole logic of the program was covered by modular tests (Except for classes that interact with static methods of the console)

## Techonogies
- С#
- .NET Core 3.1
- NUnit 3.13.0
- Moq 4.14.5

## Project's structure
The basis of the project consists of the following parts:
![Architecture](https://user-images.githubusercontent.com/58661187/90412087-f2c1b800-e0ac-11ea-9230-bf422ca50a3e.jpg)

* **`SnakeGame.Controller`** - is the main part of the project. Controller responses for controlling game speed and game state.
Because the project unites all lower modules - it defines external interfaces that they implement to follow *Dependency inversion principle*;
* **`SnakeGame.View`** - is a UI of the project. View responses for display the game elements like snake, fruit, map border of game score;
* **`SnakeGame.Input`** - is the part of the solution that is responsible for taking user input;
* **`SnakeGame.Logic`** - is the part of the solution that is responsible for handling everything related to the behavior of the game entities.

## More about:
### `System.Controller`

The System.Controller uses **Game loop pattern**. 
> A game loop runs continuously during gameplay. Each turn of the loop, it processes user input without blocking, updates the game state, and renders the game. It tracks the passage of time to control the rate of gameplay.

`SnakeGame.Controller.Game` responses for control the rate of gameplay and uses `IUpdater` for updating the game.
The 'IUpdater` interface responces for updating cycle, that consists 3 stages:
* Listen user input;
* Processing game logic;
* Display changes on UI.

For handling the 3 stages `IUpdater` uses 3 interfaces for external implementations:
* `IUserInputListener` with base implementation in `SnakeGame.Input`;
* `ILogic` with base implementation in `SnakeGame.Logic`;
* `IRenderer` with base implementation in `SnakeGame.View`.

Class `Updater` implements `IUpdate` interface. It uses **State pattern** for control 3 states of the game:
* In start menu - game waits for user input for starting the game.
* In game - game is in active state, game reacts on user input, processing logic actions like snake crawling or eating fruit and displays the changes on UI.
* In result menu - game displays the last game result and waits for user wants play again.

### `SnakeGame.Logic`
Class `GameLogic` is the main class in the `System.Logic` project. It implements the 'ILogic' interface from `SnakeGame.Controller`  and purposes for calculation the whole game step from gameplay side.
After calculating it returns changed `IGameObjects` on new positions and change own state if necessary. The class uses `ISnake` and `IFruit` interfaces for the calculation and uses `Creators` for creating them.

### `SnakeGame.Input`
The project contains only one class - `ConsoleInputListener` that implements the 'IUserListener' interface from `SnakeGame.Controller`. The class initializes listening in another thread and listen user input. If user press some button it messages KeyWasPress subscribers.

### `SnakeGame.View`
Class `ConsoleRenderer` designed to display game interface for user. The class implements the 'IRenderer' interface from `SnakeGame.Controller`. The class uses `IFramePreparer` for preparing game frames (adds color and view symbol for snake and fruit) and 'IMap' for low-level interaction with *Windows Console*.
