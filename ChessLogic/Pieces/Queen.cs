namespace ChessLogic
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;

        public override Player Color { get; }
        public Queen(Player color)
        {
            Color = color;
        }

        public override bool HasMoved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Piece Copy()
        {
            Queen copiedQueen = new Queen(Color);
            copiedQueen.HasMoved = HasMoved;
            return copiedQueen;
        }
    }
}
