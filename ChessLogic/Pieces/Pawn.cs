using ChessLogic.Moves;

namespace ChessLogic
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;

        public override Player Color { get; }
        public override bool HasMoved { get ; set ; }

        private readonly Direction forward;

        public Pawn(Player color)
        {
            Color = color;

            if(color == Player.White)
            {
                forward = Direction.North;
            }
            else if (color == Player.Black)
            {
                forward = Direction.South;
            }
        }


        public override Piece Copy()
        {
            Pawn copiedPawn = new Pawn(Color);
            copiedPawn.HasMoved = HasMoved;
            return copiedPawn;
        }

        private static bool CanMoveTo(Position position, Board board)
        {
            return Board.IsInsideBoard(position) && board.IsPositionEmpty(position);
        }

        private bool CanCaptureAt(Position position, Board board)
        {
            if(!Board.IsInsideBoard(position) || board.IsPositionEmpty(position))
            {
                return false;
            }

            return board[position].Color!= Color;
        }

        private static IEnumerable<Move> PromotionMoves(Position positionFrom, Position positionTo)
        {
            yield return new PawnPromotion(positionFrom, positionTo, PieceType.Knight);
            yield return new PawnPromotion(positionFrom, positionTo, PieceType.Bishop);
            yield return new PawnPromotion(positionFrom, positionTo, PieceType.Rook);
            yield return new PawnPromotion(positionFrom, positionTo, PieceType.Queen);
        }

        private IEnumerable<Move> ForwardMoves(Position positionFrom, Board board)
        {
            Position oneMovePosition = positionFrom + forward;

            if(CanMoveTo(oneMovePosition, board))
            {
                if(oneMovePosition.Row == 0 || oneMovePosition.Row == 7)
                {
                    foreach(Move promotionMove in PromotionMoves(positionFrom, oneMovePosition))
                    {
                        yield return promotionMove;
                    }
                }
                else
                {
                    yield return new NormalMove(positionFrom, oneMovePosition);
                }

                yield return new NormalMove(positionFrom, oneMovePosition);

                Position twoMovesPosition = oneMovePosition + forward;

                if(!HasMoved && CanMoveTo(twoMovesPosition, board))
                {
                    yield return new DoublePawn(positionFrom, twoMovesPosition);
                }
            }
        }

        private IEnumerable<Move> DiagonalMoves(Position positionFrom, Board board)
        {
            foreach (Direction dir in new Direction[] {Direction.West, Direction.East})
            {
                Position positionTo = positionFrom + forward + dir;

                if(positionTo == board.GetPawnSkipPosition(Color.GetOpponent()))
                {
                    yield return new EnPassant(positionFrom, positionTo);
                }
                else if(CanCaptureAt(positionTo,board))
                {
                    if (positionTo.Row == 0 || positionTo.Row == 7)
                    {
                        foreach (Move promotionMove in PromotionMoves(positionFrom, positionTo))
                        {
                            yield return promotionMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(positionFrom, positionTo);
                    }
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position positionFrom, Board board)
        {
            return ForwardMoves(positionFrom, board).Concat(DiagonalMoves(positionFrom, board));
        }

        public override bool CanCaptureOpponentKing(Position positionFrom, Board board)
        {
            return DiagonalMoves(positionFrom, board).Any(move =>
            {
                Piece piece = board[move.ToPosition];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
