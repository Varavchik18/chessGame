﻿namespace ChessLogic
{
    public class CountingPieces
    {
        private readonly Dictionary<PieceType, int> whiteCount = new();
        private readonly Dictionary<PieceType, int> blackCount = new();

        public CountingPieces()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                whiteCount[type] = 0; 
                blackCount[type] = 0;
            }
        }

        public int TotalCount { get; set; }

        public void Increment(Player color, PieceType type)
        {
            if (color == Player.White)
            {
                whiteCount[type] ++;
            } else if (color == Player.Black)
            {
                blackCount[type]++;
            }

            TotalCount ++;
        }

        public int White(PieceType type)
        {
            return whiteCount[type];
        }

        public int Black(PieceType type)
        {
            return blackCount[type];
        }
    }
}
