using ChessLogic.Moves;

namespace ChessLogic
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            {Player.White, null },
            {Player.Black, null }
        };

        public Piece this[int row, int col]
        {
            get
            {
                return pieces[row, col];
            }

            set
            {
                pieces[row, col] = value;
            }
        }

        public Piece this[Position position]
        {
            get { return this[position.Row, position.Column]; }
            set { this[position.Row, position.Column] = value; }
        }

        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            SpawnPawns();
        }

        public static bool IsInsideBoard(Position position)
        {
            return position.Row >= 0 && position.Row < 8 && position.Column >= 0 && position.Column < 8;
        }

        public bool IsPositionEmpty(Position position)
        {
            return this[position] == null;
        }

        private void SpawnPawns()
        {
            for (int a = 0; a < 8; a++)
            {
                this[1, a] = new Pawn(Player.Black);
                this[6, a] = new Pawn(Player.White);
            }
        }

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position position)
        {
            pawnSkipPositions[player] = position;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Position position = new Position(row, column);
                    if (!IsPositionEmpty(position))
                    {
                        yield return position;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(position => this[position].Color == player);
        }

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.GetOpponent()).Any(position =>
            {
                Piece piece = this[position];
                return piece.CanCaptureOpponentKing(position, this);
            });
        }

        public Board Copy()
        {
            Board copiedBoard = new Board();

            foreach (Position position in PiecePositions())
            {
                copiedBoard[position] = this[position].Copy();
            }

            return copiedBoard;
        }

        public CountingPieces CountPieces()
        {
            CountingPieces counting = new CountingPieces();

            foreach (Position position in PiecePositions())
            {
                Piece piece = this[position];

                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            CountingPieces counting = CountPieces();

            return IsKingVsKing(counting) || IsKingKnightVsKing(counting) || IsKingBishopVsKingBishop(counting) || IsKingBishopVsKing(counting);
        }

        private static bool IsKingVsKing(CountingPieces counting)
        {
            return counting.TotalCount == 2;
        }

        private static bool IsKingBishopVsKing(CountingPieces counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVsKing(CountingPieces counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        private bool IsKingBishopVsKingBishop(CountingPieces counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }

            Position wBishopPos = FindPiece(Player.White, PieceType.Bishop);
            Position bBishopPos = FindPiece(Player.Black, PieceType.Bishop);

            return wBishopPos.GetSquareColor() == bBishopPos.GetSquareColor();
        }

        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }

        private bool IsUnmovedKingAndRook(Position kingPosition, Position rookPosition)
        {
            if (IsPositionEmpty(kingPosition) || IsPositionEmpty(rookPosition)) return false;

            Piece king = this[kingPosition];
            Piece rook = this[rookPosition];

            return king.Type == PieceType.King &&
                rook.Type == PieceType.Rook &&
                !king.HasMoved &&
                !rook.HasMoved;
        }

        public bool CatleRightKingSide(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
                _ => false
            };
        }

        public bool CastleRightQueenSide(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false
            };
        }

        private bool HasPawnInPosition(Player player, Position[] pawnPositions, Position skipPosition)
        {
            foreach (Position position in pawnPositions.Where(IsInsideBoard))
            {
                Piece piece = this[position];
                if (piece == null || piece.Color != player || piece.Type != PieceType.Pawn)
                {
                    continue;
                }

                EnPassant move = new EnPassant(position, skipPosition);
                if (move.IsLegal(this))
                {
                    return true;
                }

            }
            return false;
        }

        public bool CanCaptureEnPassant(Player player)
        {
            Position skipPosition = GetPawnSkipPosition(player.GetOpponent());

            if (skipPosition == null)
            {
                return false;
            }

            Position[] pawnPositions = player switch
            {
                Player.White => new Position[] { skipPosition + Direction.SouthEast, skipPosition + Direction.SouthWest },
                Player.Black => new Position[] { skipPosition + Direction.NorthEast, skipPosition + Direction.NorthWest },
                _ => Array.Empty<Position>()
            };

            return HasPawnInPosition(player, pawnPositions, skipPosition);
        }
    }
}
