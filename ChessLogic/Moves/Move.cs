namespace ChessLogic
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPosition { get; }
        public abstract Position ToPosition { get; }
        public abstract void Execute(Board board);
        public virtual bool IsLegal(Board board)
        {
            Player player = board[FromPosition].Color;
            Board copyBoard = board.Copy();

            Execute(copyBoard);
            return !copyBoard.IsInCheck(player);
        }
    }
}
