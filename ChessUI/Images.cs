using ChessLogic;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessUI
{
    public static class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new()
        {
            {PieceType.Pawn, LoadImage("Assets/PawnW.png") },
            {PieceType.Bishop, LoadImage("Assets/BishopW.png") },
            {PieceType.Knight, LoadImage("Assets/KnightW.png") },
            {PieceType.Rook, LoadImage("Assets/RookW.png") },
            {PieceType.King, LoadImage("Assets/KingW.png") },
            {PieceType.Queen, LoadImage("Assets/QueenW.png") }
        };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            {PieceType.Pawn, LoadImage("Assets/PawnB.png") },
            {PieceType.Bishop, LoadImage("Assets/BishopB.png") },
            {PieceType.Knight, LoadImage("Assets/KnightB.png") },
            {PieceType.Rook, LoadImage("Assets/RookB.png") },
            {PieceType.King, LoadImage("Assets/KingB.png") },
            {PieceType.Queen, LoadImage("Assets/QueenB.png") }
        };

        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType pieceType)
        {
            return color switch
            {
                Player.White => whiteSources[pieceType],
                Player.Black => blackSources[pieceType],
                _ => null,
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null) return null;

            return GetImage(piece.Color, piece.Type);
        }
    }
}
