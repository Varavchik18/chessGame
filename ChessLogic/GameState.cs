namespace ChessLogic
{
    public class GameState
    {

        public Board Board { get; }
        public Player PlayerToMove { get; private set; }
        public Result Result { get; private set; } = null;

        private int NoCaptureOrPawnMovesCounter = 0;

        public GameState(Board board, Player playerToMove)
        {
            Board = board;
            PlayerToMove = playerToMove;
        }
        public IEnumerable<Move> LegalMoveForPiece(Position position)
        {
            if (Board.IsPositionEmpty(position) || Board[position].Color != PlayerToMove)
            {
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[position];
            IEnumerable<Move> moveVariants = piece.GetMoves(position, Board);

            return moveVariants.Where(move => move.IsLegal(Board));
        }


        public void MakeMove(Move move)
        {
            Board.SetPawnSkipPosition(PlayerToMove, null);

            bool captureOrPawnMove = move.Execute(Board);
            if (captureOrPawnMove)
            {
                NoCaptureOrPawnMovesCounter = 0;
            }
            else NoCaptureOrPawnMovesCounter++;
            PlayerToMove = PlayerToMove.GetOpponent();
            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            IEnumerable<Move> moveVariants = Board.PiecePositionsFor(player).SelectMany(position =>
            {
                Piece piece = Board[position];
                return piece.GetMoves(position, Board);
            });

            return moveVariants.Where(move => move.IsLegal(Board));
        }

        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(PlayerToMove).Any())
            {
                if (Board.IsInCheck(PlayerToMove))
                {
                    Result = Result.Win(PlayerToMove.GetOpponent());
                }
                else
                {
                    Result = Result.Draw(ReasonOfTheEnd.Stalemate);
                }
            }
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(ReasonOfTheEnd.InsufficientMaterial);
            }
            else if (FiftyMoveRule())
            {
                Result = Result.Draw(ReasonOfTheEnd.FiftyMoveRole);
            }
        }


        public bool IsGameOver()
        {
            return Result != null;
        }

        private bool FiftyMoveRule()
        {
            int fullMoves = NoCaptureOrPawnMovesCounter / 2;
            return fullMoves == 50;
        }
    }
}
