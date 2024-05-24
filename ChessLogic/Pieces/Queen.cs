namespace ChessLogic
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;
        public Direction[] directions = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.SouthWest,
            Direction.NorthWest
        };

        public override Player Color { get; }
        public Queen(Player color)
        {
            Color = color;
        }

        public override bool HasMoved { get; set; }

        public override Piece Copy()
        {
            Queen copiedQueen = new Queen(Color);
            copiedQueen.HasMoved = HasMoved;
            return copiedQueen;
        }

        public override IEnumerable<Move> GetMoves(Position positionFrom, Board board)
        {
            return MovePositionsInDirections(positionFrom, board, directions).Select(positionTo => new NormalMove(positionFrom, positionTo));
        }
    }
}
