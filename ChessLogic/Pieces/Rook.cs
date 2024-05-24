namespace ChessLogic
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;

        public override Player Color { get; }
        public Rook(Player color)
        {
            Color = color;
        }

        public override bool HasMoved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Piece Copy()
        {
            Rook copiedRook = new Rook(Color);
            copiedRook.HasMoved = HasMoved;
            return copiedRook;
        }
    }
}
