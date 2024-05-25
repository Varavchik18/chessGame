namespace ChessLogic.Moves
{
    public class EnPassant : Move
    {
        public override MoveType Type => MoveType.EnPassant;

        public override Position FromPosition { get; }

        public override Position ToPosition { get; }

        private readonly Position capturePosition;

        public EnPassant(Position fromPosition, Position toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition;

            capturePosition = new Position(fromPosition.Row, toPosition.Column);
        }

        public override void Execute(Board board)
        {
            new NormalMove(FromPosition, ToPosition).Execute(board);
            board[capturePosition] = null;
        }
    }
}
