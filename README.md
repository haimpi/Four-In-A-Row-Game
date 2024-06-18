# Four in a Row Game

## Overview
Welcome to the Four in a Row game! This console application allows you to play the classic connect-four game against a friend or a computer. The objective is to be the first to get four of your pieces in a row, either horizontally, vertically, or diagonally.

## Features
- **Game Formats**: Play against another player or the computer.
- **Customizable Board Size**: Choose the number of rows and columns (4 to 8).
- **AI Opponent**: Play against a smart computer that uses the MiniMax algorithm to make its moves.
- **Game Continuation**: Option to play multiple rounds with scores tracked.

### Running the Game
1. Run the `UiFourInARowConsole` application.
2. Follow the on-screen prompts to:
    - Select the board size.
    - Choose the game format (Player vs Player or Player vs Computer).
3. Take turns entering the column number where you want to drop your piece.
4. To quit the game, enter `Q`.

## How to Play
- **Player vs Player**: Player 1 starts the game, followed by Player 2. Players take turns dropping their pieces into one of the columns.
- **Player vs Computer**: You start the game, followed by the computer's move. The computer uses a smart algorithm to choose its moves.
- **Winning the Game**: The first player to connect four pieces in a row (horizontally, vertically, or diagonally) wins the round.
- **Ending the Round**: If the board is full and no player has won, the game ends in a draw.

## Code Structure
### UiFourInARowConsole.cs
- Manages the user interface and game flow.
- Handles user inputs, draws the game board, and checks for the end of the round.

### FourInARowLogic.cs
- Contains the game logic and rules.
- Manages the game state, players, and board interactions.

### AiMoveLogic.cs
- Implements the AI for the computer player.
- Uses the MiniMax algorithm to determine the best moves for the computer.

### GameBoard.cs
- Represents the game board and provides methods to interact with it.
- Includes methods for validating moves and checking for win conditions.

## Example Gameplay
```plaintext
Welcome to the Four In A Row Game!

You have the option to play against a friend or against the smart computer.

Instructions:

- If playing against a friend, player1 will start and then it's random.
- If playing against the computer, you always start.

Let's begin!
Please enter numbers of Rows you want between 4 to 8: 6
Please enter numbers of Cols you want between 4 to 8: 7
Please select game format:
1. Player Vs Player
2. Player Vs Computer
1
  1   2   3   4   5   6   7 
|   |   |   |   |   |   |   |
=============================
|   |   |   |   |   |   |   |
=============================
|   |   |   |   |   |   |   |
=============================
|   |   |   |   |   |   |   |
=============================
|   |   |   |   |   |   |   |
=============================
|   |   |   |   |   |   |   |
Player1's turn. Enter column number:
```

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
