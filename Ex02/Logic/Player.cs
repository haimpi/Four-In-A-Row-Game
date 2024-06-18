using System;
using Ex02.Logic.Enums;

namespace Ex02.Logic
{
    public class Player
    {
        private readonly eCellSign r_Sign;
        private int m_NumOfWins;
        private readonly bool r_IsPc;
        private readonly string r_PlayerName;

        public Player(eCellSign i_Sign, string i_PlayerName, bool i_IsPc)
        {
            r_Sign = i_Sign;
            m_NumOfWins = 0;
            r_IsPc = i_IsPc;
            r_PlayerName = i_PlayerName;
        }

        public bool IsPc
        {
            get
            {
                return r_IsPc;
            }
        }

        public eCellSign Sign
        {
            get
            {
                return r_Sign;
            }
        }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public int NumOfWins
        {
            get
            {
                return m_NumOfWins;
            }
        }

        public void IncrementWins()
        {
            m_NumOfWins++;
        }
    }
}