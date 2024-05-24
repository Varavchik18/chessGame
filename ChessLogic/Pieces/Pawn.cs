namespace ChessLogic
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;

        public override Player Color { get; }

        public Pawn(Player color)
        {
            Color = color;
        }

        public override bool HasMoved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Piece Copy()
        {
            Pawn copiedPawn = new Pawn(Color);
            copiedPawn.HasMoved = HasMoved;
            return copiedPawn;
        }
    }
}
