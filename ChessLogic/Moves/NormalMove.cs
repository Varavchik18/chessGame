namespace ChessLogic
{
    public class NormalMove : Move
    {
        public NormalMove(Position fromPosition, Position toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }

        public override MoveType Type => MoveType.Normal;
        public override Position FromPosition { get; }

        public override Position ToPosition { get; }

        public override void Execute(Board board)
        {
            Piece piece = board[FromPosition];
            board[ToPosition] = piece;
            board[FromPosition] = null;

            piece.HasMoved = true;
        }
    }
}
