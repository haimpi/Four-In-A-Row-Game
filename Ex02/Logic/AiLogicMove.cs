using System;
using System.Collections.Generic;
using System.Linq;
using Ex02.Logic.Enums;

namespace Ex02.Logic
{
    public class AiMoveLogic
    {
        private const int k_SequenceOfSameShapeToWin = 4;
        private const int k_WinningScore = 100;
        private const int k_ThreeInARowScore = 5;
        private const int k_TwoInARowScore = 2;
        private const int k_BlockedWinScore = 4;
        private const int k_UndefinedRowOrColumn = -1;

        public int[] MiniMaxAlgorithm(GameBoard i_Board, int i_Depth = 7, int i_Alpha = int.MinValue, int i_Beta = int.MaxValue, bool i_IsMaximizingPlayer = true)
        {
            int[] bestMove = new int[2]; // [0] bestScore, [1] bestColumn
            List<int> possibleMoves = i_Board.GetValidLocations();
            bestMove[1] = k_UndefinedRowOrColumn;
            bestMove[0] = i_IsMaximizingPlayer ? int.MinValue : int.MaxValue;

            if (FourInARowLogic.CheckIfWin(i_Board, eCellSign.PlayerO))
            {
                bestMove[0] = k_WinningScore;
            }
            else if (FourInARowLogic.CheckIfWin(i_Board, eCellSign.PlayerX))
            {
                bestMove[0] = -k_WinningScore; 
            }
            else if (possibleMoves.Count == 0)
            {
                bestMove[0] = 0;
            }
            else if (i_Depth == 0)
            {
                bestMove[0] = scorePosition(i_Board, eCellSign.PlayerO);
            }
            else
            {
                bestMove[1] = possibleMoves.First();
                eCellSign currentPiece = i_IsMaximizingPlayer ? eCellSign.PlayerO : eCellSign.PlayerX;

                foreach (int position in possibleMoves)
                {
                    int validRow = i_Board.ValidRow(position);
                    i_Board.SetCellValue(validRow, position, currentPiece);
                    int[] newMove = MiniMaxAlgorithm(i_Board, i_Depth - 1, i_Alpha, i_Beta, !i_IsMaximizingPlayer);
                    i_Board.RemoveFromCell(validRow, position);

                    if (i_IsMaximizingPlayer)
                    {
                        if (newMove[0] > bestMove[0])
                        {
                            bestMove[0] = newMove[0];
                            bestMove[1] = position;
                        }

                        i_Alpha = Math.Max(i_Alpha, newMove[0]);
                    }
                    else
                    {
                        if (newMove[0] < bestMove[0])
                        {
                            bestMove[0] = newMove[0];
                            bestMove[1] = position;
                        }

                        i_Beta = Math.Min(i_Beta, newMove[0]);
                    }

                    if (i_Beta <= i_Alpha)
                    {
                        break;
                    }
                }
            }

            return bestMove;
        }

        private int scorePosition(GameBoard i_Board, eCellSign i_Sign)
        {
            int score = 0;

            score += scoreCenter(i_Board, i_Sign)
            + scoreHorizontal(i_Board, i_Sign)
            + scoreVertical(i_Board, i_Sign)
            + scoreDiagonals(i_Board, i_Sign);

            return score;
        }

        private static int scoreCenter(GameBoard i_Board, eCellSign i_Sign)
        {
            int arrayCounter = 0;
            int centerCol = i_Board.Columns / 2;
            bool colIsEven = i_Board.Columns % 2 == 0;

            for (int i = 0; i < i_Board.Rows; i++)
            {
                if (i_Board.GetCellValue(i, centerCol) == i_Sign)
                {
                    arrayCounter++;
                }

                if (colIsEven && i_Board.GetCellValue(i, centerCol - 1) == i_Sign)
                {
                    arrayCounter++;
                }
            }

            return arrayCounter * 10;
        }

        private static int scoreHorizontal(GameBoard i_Board, eCellSign i_Sign)
        {
            int score = 0;

            for (int row = 0; row < i_Board.Rows; row++)
            {
                for (int col = 0; col < i_Board.Columns - 3; col++)
                {
                    eCellSign[] window = new eCellSign[k_SequenceOfSameShapeToWin];

                    for (int i = 0; i < k_SequenceOfSameShapeToWin; i++)
                    {
                        window[i] = i_Board.GetCellValue(row, col + i);
                    }

                    score += calculateWindowScore(window, i_Sign);
                }
            }

            return score;
        }

        private static int scoreVertical(GameBoard i_Board, eCellSign i_Sign)
        {
            int score = 0;

            for (int col = 0; col < i_Board.Columns; col++)
            {
                for (int row = 0; row < i_Board.Rows - 3; row++)
                {
                    eCellSign[] window = new eCellSign[k_SequenceOfSameShapeToWin];

                    for (int i = 0; i < k_SequenceOfSameShapeToWin; i++)
                    {
                        window[i] = i_Board.GetCellValue(row + i, col);
                    }

                    score += calculateWindowScore(window, i_Sign);
                }
            }

            return score;
        }

        private static int scoreDiagonals(GameBoard i_Board, eCellSign i_Sign)
        {
            int score = 0;

            for (int row = 0; row < i_Board.Rows - 3; row++)
            {
                for (int col = 0; col < i_Board.Columns - 3; col++)
                {
                    eCellSign[] window = new eCellSign[k_SequenceOfSameShapeToWin];

                    for (int i = 0; i < k_SequenceOfSameShapeToWin; i++)
                    {
                        window[i] = i_Board.GetCellValue(row + i, col + i);
                    }

                    score += calculateWindowScore(window, i_Sign);
                }
            }

            for (int row = 0; row < i_Board.Rows - 3; row++)
            {
                for (int col = 0; col < i_Board.Columns - 3; col++)
                {
                    eCellSign[] window = new eCellSign[k_SequenceOfSameShapeToWin];

                    for (int i = 0; i < k_SequenceOfSameShapeToWin; i++)
                    {
                        window[i] = i_Board.GetCellValue(row + 3 - i, col + i);
                    }

                    score += calculateWindowScore(window, i_Sign);
                }
            }

            return score;
        }

        private static int calculateWindowScore(eCellSign[] i_Window, eCellSign i_Sign)
        {
            int score = 0;
            int countPlayerSign = 0;
            int countOpponentSign = 0;
            int countBlankSign = 0;
            eCellSign opponentSign = (i_Sign == eCellSign.PlayerX) ? eCellSign.PlayerO : eCellSign.PlayerX;

            foreach (eCellSign cellSign in i_Window)
            {
                if (cellSign == i_Sign)
                {
                    countPlayerSign++;
                }
                else if (cellSign == opponentSign)
                {
                    countOpponentSign++;
                }
                else
                {
                    countBlankSign++;
                }
            }

            switch (countPlayerSign)
            {
                case k_SequenceOfSameShapeToWin:
                    score += k_WinningScore;
                    break;
                case k_SequenceOfSameShapeToWin - 1:
                    score += k_ThreeInARowScore;
                    break;
                case k_SequenceOfSameShapeToWin - 2:
                    score += k_TwoInARowScore;
                    break;
            }

            if (countOpponentSign == (k_SequenceOfSameShapeToWin - 1) && countBlankSign == 1)
            {
                score -= k_BlockedWinScore;
            }

            return score;
        }
    }
}