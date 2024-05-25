
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

        public override bool HasMoved { get; set; }

        private static bool IsUnmovedRook(Position position, Board board)
        {
            if(board.IsPositionEmpty(position)) return false;
            
            Piece piece = board[position];
            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AllSquaresEmptyOnTheWayToCastle(IEnumerable<Position> positions, Board board)
        {
            return positions.All(positions => board.IsPositionEmpty(positions));
        }

        private bool CanCastleKingSide(Position positionFrom, Board board)
        {
            if (HasMoved)
            {
                return false;
            }


            Position rookPosition = new Position(positionFrom.Row, 7);
            Position[] betweenPositions = new Position[] { new(positionFrom.Row, 5), new(positionFrom.Row, 6) };

            return IsUnmovedRook(rookPosition, board) && AllSquaresEmptyOnTheWayToCastle(betweenPositions, board);
        }

        private bool CanCastleQueenSide(Position positionFrom, Board board)
        {
            if (HasMoved) return false;

            Position rookPosition = new Position(positionFrom.Row, 0);
            Position[] betweenPositions = new Position[] { new(positionFrom.Row, 1), new(positionFrom.Row, 2), new(positionFrom.Row, 3) };

            return IsUnmovedRook(rookPosition, board) && AllSquaresEmptyOnTheWayToCastle(betweenPositions, board);
        }

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

            if(CanCastleKingSide(positionFrom, board))
            {
                yield return new CastleMove(MoveType.CastleKingSide, positionFrom);
            }

            if(CanCastleQueenSide((Position)positionFrom, board))
            {
                yield return new CastleMove(MoveType.CastleQueenSide, positionFrom);
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


        public override bool CanCaptureOpponentKing(Position positionFrom, Board board)
        {
            return MovePositions(positionFrom, board).Any(positionTo =>
            {
                Piece piece = board[positionTo];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
