namespace ChessLogic
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;

        public override Player Color { get; }
        public Knight(Player color)
        {
            Color = color;
        }

        public override bool HasMoved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Piece Copy()
        {
            Knight copiedKnight = new Knight(Color);
            copiedKnight.HasMoved = HasMoved;
            return copiedKnight;
        }
    }
}
