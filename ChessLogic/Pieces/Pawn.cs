﻿namespace ChessLogic
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;

        public override Player Color { get; }

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

        public override bool HasMoved { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        private IEnumerable<Move> ForwardMoves(Position positionFrom, Board board)
        {
            Position oneMovePosition = positionFrom + forward;

            if(CanMoveTo(oneMovePosition, board))
            {
                yield return new NormalMove(positionFrom, oneMovePosition);

                Position twoMovesPosition = oneMovePosition + forward;

                if(!HasMoved && CanMoveTo(twoMovesPosition, board))
                {
                    yield return new NormalMove(positionFrom, twoMovesPosition);
                }
            }
        }

        private IEnumerable<Move> DiagonalMoves(Position positionFrom, Board board)
        {
            foreach (Direction dir in new Direction[] {Direction.West, Direction.East})
            {
                Position positionTo = positionFrom + forward + dir;

                if(CanCaptureAt(positionTo,board))
                {
                    yield return new NormalMove(positionFrom, positionTo);
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position positionFrom, Board board)
        {
            return ForwardMoves(positionFrom, board).Concat(DiagonalMoves(positionFrom, board));
        }
    }
}
