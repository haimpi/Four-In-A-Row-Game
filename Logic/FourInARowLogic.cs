using System;
using Ex02.Logic.Enums;

namespace Ex02.Logic
{
    public class FourInARowLogic
    {
        public const int k_MaxSize = 8;
        public const int k_MinSize = 4;
        private readonly Player r_Player1 = null;
        private readonly Player r_Player2 = null;
        private Player m_CurrentPlayer = null;
        private readonly AiMoveLogic r_AiMoveLogic = null;
        private readonly GameBoard r_Board = null;
        public bool m_IsRoundEnded;
        private readonly eGameFormat r_GameFormat;

        public FourInARowLogic(int i_Rows, int i_Cols, int i_UserFormat)
        {
            r_Board = new GameBoard(i_Rows, i_Cols);
            r_Player1 = new Player(eCellSign.PlayerX, "Player1", false);
            m_CurrentPlayer = r_Player1;
            m_IsRoundEnded = false;
            r_GameFormat = (eGameFormat)i_UserFormat;

            if (r_GameFormat == eGameFormat.PlayerVsComputer)
            {
                r_Player2 = new Player(eCellSign.PlayerO, "PC", true);
                r_AiMoveLogic = new AiMoveLogic();
            }
            else
            {
                r_Player2 = new Player(eCellSign.PlayerO, "Player2", false);
            }
        }

        public bool RoundEnded
        {
            get
            {
                return m_IsRoundEnded;
            }
            set
            {
                m_IsRoundEnded = value;
            }
        }

        public static bool IsValidSizeForGameBoard(int i_UserSize)
        {
            return i_UserSize >= 4 && i_UserSize <= 8;
        }

        public static bool IsValidFormatGame(int i_UserPreferenceFormat)
        {
            return Enum.IsDefined(typeof(eGameFormat), i_UserPreferenceFormat);
        }

        public void PcMove(eCellSign i_PcSign)
        {
            bool isValidMove = false;
            int[] bestMove = new int[2];//[0] bestScore, [1] bestColumn

            while (!isValidMove)
            {
                bestMove = r_AiMoveLogic.MiniMaxAlgorithm(r_Board);
                isValidMove = MakeMove(bestMove[1] + 1, i_PcSign);
            }
        }

        public bool MakeMove(int i_ColGetCellValue, eCellSign i_UserSign)
        {
            bool isValidMove = false;
            int availableRow = r_Board.ValidRow(i_ColGetCellValue - 1); // Adjusting for 0-based indexing

            if (availableRow != -1)
            {
                r_Board.SetCellValue(availableRow, i_ColGetCellValue - 1, i_UserSign);
                isValidMove = true; 
            }
            
            return isValidMove;
        }

        public void PlayerRetired()
        {
            SwitchPlayer();
            m_CurrentPlayer.IncrementWins();
        }

        public void SetStarter()
        {
            r_Board.InitializeBoard();

            if (eGameFormat.PlayerVsComputer == r_GameFormat)
            {
                m_CurrentPlayer = r_Player1;
            }
            else
            {
                int starter = new Random().Next(0,2);

                m_CurrentPlayer = starter == 0 ? r_Player1 : r_Player2;
            }
        }

        public void SwitchPlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == r_Player1 ? r_Player2 : r_Player1;
        }

        public bool IsDraw()
        {
            return r_Board.IsFullBoard();
        }

        public bool CheckIfWin()
        {
            bool result = false;
            eCellSign playerSign = m_CurrentPlayer.Sign;

            if (r_Board.CheckDiagonalFourSequence(playerSign) || r_Board.CheckHorizontalFourSequence(playerSign) ||
                r_Board.CheckVerticalFourSequence(playerSign))
            {
                result = true;
                m_CurrentPlayer.IncrementWins();
            }

            SwitchPlayer();

            return result;
        }
        
        public static bool CheckIfWin(GameBoard i_GameBoard, eCellSign i_Sign)
        {
            return i_GameBoard.CheckDiagonalFourSequence(i_Sign) || i_GameBoard.CheckVerticalFourSequence(i_Sign) ||
                i_GameBoard.CheckHorizontalFourSequence(i_Sign);
        }

        public int[] PlayersWins()
        {
            return new int[] {r_Player1.NumOfWins, r_Player2.NumOfWins};
        }

        public string[] PlayersNames()
        {
            return new string[] {r_Player1.PlayerName, r_Player2.PlayerName};
        }

        public string CurrentPlayerName()
        {
            return m_CurrentPlayer.PlayerName;
        }

        public eCellSign CurrentPlayerSign()
        {
            return m_CurrentPlayer.Sign;
        }

        public bool CurrentPlayerIsPc()
        {
            return m_CurrentPlayer.IsPc;
        }

        public eCellSign GetCellValue(int i_Row, int i_Col)
        {
            return r_Board.GetCellValue(i_Row, i_Col);
        }

        public int GetGameBoardColumns()
        {
            return r_Board.Columns;
        }

        public int GetGameBoardRows()
        {
            return r_Board.Rows;
        }

        public enum eGameFormat
        {
            PlayerVsPlayer = 1,
            PlayerVsComputer
        }
    }
}