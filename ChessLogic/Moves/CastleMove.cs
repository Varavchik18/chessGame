namespace ChessLogic
{
    public class CastleMove : Move
    {
        public override MoveType Type { get; }

        public override Position FromPosition { get; }

        public override Position ToPosition { get; }

        private readonly Direction kingMoveDirection;
        private readonly Position rookFromPosition;
        private readonly Position rookToPosition;

        public CastleMove(MoveType type, Position kingPosition)
        {
            Type = type;
            FromPosition = kingPosition;

            if (type == MoveType.CastleKingSide)
            {
                kingMoveDirection = Direction.East;
                ToPosition = new Position(kingPosition.Row, 6);
                rookFromPosition = new Position(kingPosition.Row, 7);
                rookToPosition = new Position(kingPosition.Row, 5);
            }
            else if (type == MoveType.CastleQueenSide)
            {
                kingMoveDirection = Direction.West;

                ToPosition = new Position(kingPosition.Row, 2);
                rookFromPosition = new Position(kingPosition.Row, 0);
                rookToPosition = new Position(kingPosition.Row, 3);
            }
        }

        public override bool Execute(Board board)
        {
            new NormalMove(FromPosition, ToPosition).Execute(board);
            new NormalMove(rookFromPosition, rookToPosition).Execute(board);

            return false;
        }

        public override bool IsLegal(Board board)
        {
            Player player = board[FromPosition].Color;

            if (board.IsInCheck(player))
            {
                return false;
            }

            Board copy = board.Copy();
            Position kingPositionInCopyBoard = FromPosition;

            for (int i = 0; i < 2; i++)
            {
                new NormalMove(kingPositionInCopyBoard, kingPositionInCopyBoard + kingMoveDirection).Execute(copy);
                kingPositionInCopyBoard += kingMoveDirection;

                if (copy.IsInCheck(player))
                {
                    return false;
                }

            }
            return true;
        }
    }
}
