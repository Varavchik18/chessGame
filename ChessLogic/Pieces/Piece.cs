namespace ChessLogic
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public abstract bool HasMoved { get; set; }

        public abstract Piece Copy();
    }
}
