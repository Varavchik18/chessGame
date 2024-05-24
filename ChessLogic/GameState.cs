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

        public IEnumerable<Move> LegalMoveForPiece(Position position)
        {
            if(Board.IsPositionEmpty(position) || Board[position].Color != PlayerToMove)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[position];
            return piece.GetMoves(position, Board);
        }


        public void MakeMove(Move move)
        {
            move.Execute(Board);
            PlayerToMove = PlayerToMove.GetOpponent();
        }
    }
}
