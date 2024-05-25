using System.Text;

namespace ChessLogic
{
    public class StateString
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public StateString(Player currentPlayer, Board board)
        {
            AddPiecePlacement(board);
            stringBuilder.Append(' ');
            AddCurrenPlayerInformation(currentPlayer);
            stringBuilder.Append(' ');
            AddCastlingRights(board);
            stringBuilder.Append(' ');
            AddEnPassantInformation(board, currentPlayer);
            stringBuilder.Append(' ');
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        private static char PieceChar(Piece piece)
        {
            char c = piece.Type switch
            {
                PieceType.Pawn => 'p',
                PieceType.Knight => 'n',
                PieceType.Bishop => 'b',
                PieceType.Rook => 'r',
                PieceType.Queen => 'q',
                PieceType.King => 'k',
                _ => ' '
            };

            if (piece.Color == Player.White)
            {
                return char.ToUpper(c);
            }

            return c;
        }

        private void AddRowData(Board board, int row)
        {
            int emptySquares = 0;

            for (int column = 0; column < 8; column++)
            {
                if (board[row, column] == null)
                {
                    emptySquares++;
                    continue;
                }

                if (emptySquares > 0)
                {
                    stringBuilder.Append(emptySquares);
                    emptySquares = 0;
                }

                stringBuilder.Append(PieceChar(board[row, column]));
            }

            if (emptySquares > 0)
            {
                stringBuilder.Append(emptySquares);
            }
        }

        private void AddPiecePlacement(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                if (row != 0)
                {
                    stringBuilder.Append('/');
                }
                AddRowData(board, row);
            }
        }

        private void AddCurrenPlayerInformation(Player currentPlayer)
        {
            if (currentPlayer == Player.White)
            {
                stringBuilder.Append('w');
            }
            else
            {
                stringBuilder.Append('b');
            }
        }

        private void AddCastlingRights(Board board)
        {
            bool castleWhiteKingSide = board.CatleRightKingSide(Player.White);
            bool castleWhiteQueenSide = board.CastleRightQueenSide(Player.White);

            bool castleBlackKingSide = board.CatleRightKingSide(Player.Black);
            bool castleBlackQueenSide = board.CastleRightQueenSide(Player.Black);

            if (!(castleWhiteKingSide || castleWhiteQueenSide || castleBlackQueenSide || castleBlackKingSide))
            {
                stringBuilder.Append('-');
                return;
            }
            else if (castleWhiteKingSide)
            {
                stringBuilder.Append('K');
            }
            else if (castleWhiteQueenSide)
            {
                stringBuilder.Append('Q');
            }
            else if (castleBlackKingSide)
            {
                stringBuilder.Append('k');
            }
            else if (castleBlackQueenSide)
            {
                stringBuilder.Append('q');
            }
        }

        private void AddEnPassantInformation(Board board, Player currentPlayer)
        {
            if (!board.CanCaptureEnPassant(currentPlayer))
            {
                stringBuilder.Append('-'); 
                return;
            }

            Position position = board.GetPawnSkipPosition(currentPlayer.GetOpponent());
            char file = (char)('a' + position.Column);

            int rank = 8 - position.Row;
            stringBuilder.Append(file);
            stringBuilder.Append(rank);
        }
    }
}
