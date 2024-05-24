
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

        private static IEnumerable<Position> GetPotentialTwoPositions(Position positionFrom)
        {
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hDir in new Direction[] { Direction.West, Direction.East })
                {
                    yield return positionFrom + 2 * vDir + hDir;
                    yield return positionFrom + 2 * hDir + vDir;
                }
            }
        }

        private IEnumerable<Position> MovePositions(Position positionFrom, Board board)
        {
            return GetPotentialTwoPositions(positionFrom).Where(position => Board.IsInsideBoard(position) && (board.IsPositionEmpty(position) || board[position].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position positionFrom, Board board)
        {
            return MovePositions(positionFrom, board).Select(positionTo => new NormalMove(positionFrom, positionTo));
        }
    }
}
