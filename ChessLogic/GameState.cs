namespace ChessLogic
{
    public class GameState
    {
        public GameState(Board board, Player playerToMove)
        {
            Board = board;
            PlayerToMove = playerToMove;
        }

        public Board Board { get; }
        public Player PlayerToMove { get; private set; }
    }
}
