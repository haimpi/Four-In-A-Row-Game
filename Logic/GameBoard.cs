using System.Collections.Generic;
using Ex02.Logic.Enums;

namespace Ex02.Logic
{
    public class GameBoard
    {
        private readonly eCellSign[,] r_BoardMatrix = null;

        public GameBoard(int i_Rows, int i_Cols)
        {
            Rows = i_Rows;
            Columns = i_Cols;
            r_BoardMatrix = new eCellSign[Rows, Columns];
            InitializeBoard();
        }

        public int Rows { get; }

        public int Columns { get; }

        public void InitializeBoard()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    r_BoardMatrix[i, j] = eCellSign.Blank;
                }
            }
        }

        public eCellSign GetCellValue(int i_Row, int i_Col)
        {
            return r_BoardMatrix[i_Row, i_Col];
        }

        public bool IsFullBoard()
        {
            bool isNotFullBoard = false;

            for (int col = 0; col < Columns; col++)
            {
                if (GetCellValue(0, col) == eCellSign.Blank)
                {
                    isNotFullBoard = true;
                    break; 
                }
            }

            return !isNotFullBoard;
        }

        public void SetCellValue(int i_Row, int i_Col, eCellSign i_Sign)
        {
            r_BoardMatrix[i_Row, i_Col] = i_Sign;
        }

        public bool CheckHorizontalFourSequence(eCellSign i_CurrentPlayerSign)
        {
            bool result = false;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns - 3; j++)
                {
                    if (r_BoardMatrix[i, j] == i_CurrentPlayerSign &&
                        r_BoardMatrix[i, j + 1] == i_CurrentPlayerSign &&
                        r_BoardMatrix[i, j + 2] == i_CurrentPlayerSign &&
                        r_BoardMatrix[i, j + 3] == i_CurrentPlayerSign)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public bool CheckVerticalFourSequence(eCellSign i_CurrentPlayerSign)
        {
            bool result = false;

            for (int i = 0; i < Rows - 3; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (r_BoardMatrix[i, j] == i_CurrentPlayerSign &&
                        r_BoardMatrix[i + 1, j] == i_CurrentPlayerSign &&
                        r_BoardMatrix[i + 2, j] == i_CurrentPlayerSign &&
                        r_BoardMatrix[i + 3, j] == i_CurrentPlayerSign)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public bool CheckDiagonalFourSequence(eCellSign i_CurrentPlayer)
        {
            bool result = false;

            for (int i = 0; i < Rows - 3; i++)
            {
                for (int j = 0; j < Columns - 3; j++)
                {
                    if (r_BoardMatrix[i, j] == i_CurrentPlayer &&
                        r_BoardMatrix[i + 1, j + 1] == i_CurrentPlayer &&
                        r_BoardMatrix[i + 2, j + 2] == i_CurrentPlayer &&
                        r_BoardMatrix[i + 3, j + 3] == i_CurrentPlayer)
                    {
                        result = true;
                        break;
                    }

                    if (r_BoardMatrix[i, j + 3] == i_CurrentPlayer &&
                        r_BoardMatrix[i + 1, j + 2] == i_CurrentPlayer &&
                        r_BoardMatrix[i + 2, j + 1] == i_CurrentPlayer &&
                        r_BoardMatrix[i + 3, j] == i_CurrentPlayer)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public int ValidRow(int i_Col)
        {
            int availableRow = -1;

            for (int row = Rows - 1; row >= 0; row--)
            {
                if (GetCellValue(row, i_Col) == eCellSign.Blank)
                {
                    availableRow = row;
                    break;
                }
            }

            return availableRow;
        }

        public List<int> GetValidLocations()
        {
            List<int> validLocations = new List<int>();

            for (int col = 0; col < Columns; col++)
            {
                if (!IsColumnFull(col))
                {
                    validLocations.Add(col);
                }
            }

            return validLocations;
        }

        public bool IsColumnFull(int i_Col)
        {
            return r_BoardMatrix[0, i_Col] != eCellSign.Blank;
        }

        public void RemoveFromCell(int i_Row, int i_Column)
        {
            r_BoardMatrix[i_Row, i_Column] = eCellSign.Blank;
        }
    }
}