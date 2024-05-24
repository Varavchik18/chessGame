namespace ChessLogic
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;

        public override Player Color { get; }
        public Bishop (Player color)
        {
            Color = color;
        }

        public override bool HasMoved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Piece Copy()
        {
            Bishop copiedBishop = new Bishop(Color);
            copiedBishop.HasMoved = HasMoved;
            return copiedBishop;
        }
    }
}
