namespace ChessLogic
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;

        public override Player Color { get; }
        private static readonly Direction[] directions = new Direction[]
        {
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast,
            Direction.NorthWest
        };

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

        public override IEnumerable<Move> GetMoves(Position positionFrom, Board board)
        {
            return MovePositionsInDirections(positionFrom, board, directions).Select(positionTo => new NormalMove(positionFrom, positionTo));
        }
    }
}
