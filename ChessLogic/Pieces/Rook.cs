namespace ChessLogic
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public Direction[] directions = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
        };

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

        public override IEnumerable<Move> GetMoves(Position positionFrom, Board board)
        {
            return MovePositionsInDirections(positionFrom, board, directions).Select(positionTo => new NormalMove(positionFrom, positionTo));
        }
    }
}
