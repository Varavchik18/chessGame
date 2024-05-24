
namespace ChessLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        private static readonly Direction[] directions = new Direction[]
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

        public override IEnumerable<Move> GetMoves(Position positionFrom, Board board)
        {
            foreach (Position positionTo in MovePositions(positionFrom, board))
            {
                yield return new NormalMove(positionFrom, positionTo);
            }
        }

        public IEnumerable<Position> MovePositions(Position positionFrom, Board board)
        {
            foreach(Direction direction in directions)
            {
                Position positionTo = positionFrom + direction;

                if (!Board.IsInsideBoard(positionTo))
                {
                    continue;
                }

                if (board.IsPositionEmpty(positionTo) || board[positionTo].Color != Color)
                {
                    yield return positionTo;
                }
            }
        }
    }
}
