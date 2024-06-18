using Ex02.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02.Logic;
using Ex02.Logic.Enums;

namespace Ex02
{
    public class UiFourInARowConsole
    {
        private FourInARowLogic m_Game;
        private int m_Rows;
        private int m_Columns;
        public void StartGame()
        {
            displayWelcomeMessageFourInARow();
            getBoardSizeFromUser();
            getGameFormatFromUser();
            drawBoard();
            playRound();
        }

        private void displayWelcomeMessageFourInARow()
        {
            const string k_WelcomeMessage = @"
Welcome to the Four In A Row Game!

You have the option to play against a friend or against the smart computer.

Instructions:

- If playing against a friend, player1 will start and then it's random.
- If playing against the computer, you always start.

Let's begin!";

            Console.WriteLine(k_WelcomeMessage);
        }

        private void newRound()
        {
            if (m_Game.RoundEnded == true)
            {
                m_Game.RoundEnded = false;
                Console.WriteLine("Do you want to play again? press Y(Yes) or N(No)");
                bool isWantNewRound = askForNewRound();

                if (isWantNewRound)
                {
                    m_Game.SetStarter();
                    Screen.Clear();
                    drawBoard();
                    playRound();
                }
                else
                {
                    Console.WriteLine("Game is over");
                }
            }
        }

        private static bool askForNewRound()
        {
            bool isWantNewRound = false;

            while (!isWantNewRound)
            {
                string userInput = Console.ReadLine();
                if (userInput.ToUpper() == "Y")
                {
                    isWantNewRound = true;
                    break;
                }
                else if (userInput.ToUpper() == "N")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input please enter Y or N");
                }
            }

            return isWantNewRound;
        }

        private void printResult()
        {
            string[] names = m_Game.PlayersNames();
            int[] wins = m_Game.PlayersWins();
            string resultWin = string.Format(
"### {0} score: {1} VS {2} score: {3} ###",
names[0],
wins[0],
names[1],
wins[1]);

            Console.WriteLine(resultWin);
        }

        private void playRound()
        {
            do
            {
                Console.WriteLine($"{m_Game.CurrentPlayerName()}'s turn. Enter column number:");
                string userInput = Console.ReadLine();
                bool isValid = int.TryParse(userInput, out int userColumnChoice);

                if (isLeftTheGame(userInput))
                {
                    Console.WriteLine($"{m_Game.CurrentPlayerName()} won!");
                    printResult();
                    m_Game.RoundEnded = true;
                    newRound();
                    break;
                }

                if (isValid && isValidInputColumn(userColumnChoice))
                {
                    if (!m_Game.MakeMove(userColumnChoice, m_Game.CurrentPlayerSign()))
                    {
                        Console.WriteLine("This column is full, please try another one");
                    }
                    else
                    {
                        drawBoard();
                        if (checkEndOfRound())
                        {
                            break;
                        }

                        if (m_Game.CurrentPlayerIsPc())
                        {
                            m_Game.PcMove(m_Game.CurrentPlayerSign());
                            drawBoard();
                            if(checkEndOfRound())
                            {
                                break;
                            }
                        }

                        drawBoard();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid number please enter a correct Column.");
                }

            } while (true);
        }

        private bool checkEndOfRound()
        {
            bool isRoundEnded = false;
            string currentPlayerName = m_Game.CurrentPlayerName();

            if (m_Game.CheckIfWin())
            {
                Console.WriteLine($"{currentPlayerName} wins!");
                isRoundEnded = true;
            }
            else if (m_Game.IsDraw())
            {
                Console.WriteLine("Draw, end of round!");
                isRoundEnded = true;
            }

            if (isRoundEnded == true)
            {
                printResult();
                m_Game.m_IsRoundEnded = true;
                newRound();
            }

            return isRoundEnded;
        }

        private bool isValidInputColumn(int i_UserInput)
        {
            return i_UserInput >= 1 && i_UserInput <= m_Game.GetGameBoardColumns();
        }

        private bool isLeftTheGame(string i_UserInput)
        {
            bool result = i_UserInput.ToUpper() == "Q";

            if(result)
            {
                Console.WriteLine($"{m_Game.CurrentPlayerName()} left the game");
                m_Game.PlayerRetired();
            }

            return result;
        }

        private void getBoardSizeFromUser()
        {
            Console.WriteLine($"Please enter numbers of Rows you want between {FourInARowLogic.k_MinSize} to {FourInARowLogic.k_MaxSize}");
            m_Rows = getValidBoardSizeFromUser();
            Console.WriteLine($"Please enter numbers of Cols you want between  {FourInARowLogic.k_MinSize}  to  {FourInARowLogic.k_MaxSize} ");
            m_Columns = getValidBoardSizeFromUser();
        }

        private void getGameFormatFromUser()
        {
            Console.WriteLine(@"Please select game format:
1. Player Vs Player
2. Player Vs Computer");

            while (true)
            {
                bool isNumber = int.TryParse(Console.ReadLine(), out int userInput);

                if (isNumber && FourInARowLogic.IsValidFormatGame(userInput))
                {
                    m_Game = new FourInARowLogic(m_Rows, m_Columns, userInput);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input please try again.");
                }
            }
        }


        private static int getValidBoardSizeFromUser()
        {
            int userInput = 0;

            while (true)
            {
                bool isNumber = int.TryParse(Console.ReadLine(), out userInput);

                if (isNumber && FourInARowLogic.IsValidSizeForGameBoard(userInput))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Your input is invalid please choose number between 4 to 8");
                }
            }

            return userInput;
        }

        private void drawBoard()
        {
            Screen.Clear();
            int numOfRows = m_Game.GetGameBoardRows();

            for (int row = -1; row < numOfRows; row++)
            {
                if (row == -1)
                {
                    drawHeader();
                }
                else
                {
                    drawRow(row);
                }
            }
        }

        private void drawHeader()
        {
            StringBuilder headerBuilder = new StringBuilder();
            int numOfCols = m_Game.GetGameBoardColumns();

            for (int col = 0; col < numOfCols; col++)
            {
                headerBuilder.AppendFormat("  {0} ", col + 1);
            }

            Console.WriteLine(headerBuilder.ToString());
        }

        private void drawRow(int i_Row)
        {
            StringBuilder rowBuilder = new StringBuilder();
            int numOfCols = m_Game.GetGameBoardColumns();

            for (int col = 0; col < numOfCols; col++)
            {
                rowBuilder.AppendFormat("| {0} ", (char)m_Game.GetCellValue(i_Row, col));
            }
            
            rowBuilder.Append("|");
            Console.WriteLine(rowBuilder.ToString());

            drawSeparator();
        }

        private void drawSeparator()
        {
            StringBuilder separatorBuilder = new StringBuilder();
            string sepLine = "====";
            int numOfCols = m_Game.GetGameBoardColumns();

            for (int col = 0; col < numOfCols; col++)
            {
                separatorBuilder.Append(sepLine);
            }

            separatorBuilder.Append("=");
            Console.WriteLine(separatorBuilder.ToString());
        }
    }
}