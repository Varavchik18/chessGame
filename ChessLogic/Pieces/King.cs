namespace ChessLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;

        public override Player Color { get; }
        public King(Player color)
        {
            Color = color;
        }

        public override bool HasMoved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Piece Copy()
        {
            King copiedKing = new King(Color);
            copiedKing.HasMoved = HasMoved;
            return copiedKing;
        }
    }
}
