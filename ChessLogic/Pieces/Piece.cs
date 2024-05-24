namespace ChessLogic
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public abstract bool HasMoved { get; set; }

        public abstract Piece Copy();
        public abstract IEnumerable<Move> GetMoves(Position positionFrom, Board board);

        protected IEnumerable<Position> MovePositionInDirection(Position positionFrom, Board board, Direction direction)
        {
            for (Position position = positionFrom + direction; Board.IsInsideBoard(position); position += direction)
            {
                if(board.IsPositionEmpty(position))
                {
                    yield return position;
                    continue;
                }

                Piece piece = board[position];
                if(piece.Color != Color)
                {
                    yield return position;
                }

                yield break;
            }
        }


        protected IEnumerable<Position> MovePositionsInDirections(Position positionFrom, Board board, Direction[] directions)
        {
            return directions.SelectMany(direction => MovePositionInDirection(positionFrom, board, direction));
        }
    }
}
