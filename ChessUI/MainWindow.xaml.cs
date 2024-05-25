using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ChessLogic;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Image[,] pieceImages = new Image[8, 8];
        private readonly Rectangle[,] highlights = new Rectangle[8, 8];
        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

        private GameState gameState;
        private Position selectedPosition = null;
        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            gameState = new GameState(Board.Initial(), Player.White);
            DrawBoard(gameState.Board);
            SetCursor(gameState.PlayerToMove);
        }


        private void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Image image = new Image();
                    pieceImages[row, col] = image;

                    PieceGrid.Children.Add(image);

                    Rectangle highlight = new Rectangle();
                    highlights[row, col] = highlight;
                    HighlightGrid.Children.Add(highlight);

                }
            }
        }

        private void DrawBoard(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = board[row, col];
                    pieceImages[row, col].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsMenuOnScreen())
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Position position = ToSquarePosition(point);

            if(selectedPosition == null)
            {
                OnFromPositionSelected(position);
            }
            else
            {
                OnToPositionSelected(position);
            }
        }

        private void OnToPositionSelected(Position position)
        {
            selectedPosition = null;

            HideHighlights();

            if(moveCache.TryGetValue(position, out Move move))
            {
                HandleMove(move);
            }
        }

        private void HandleMove(Move move)
        {
            gameState.MakeMove(move);
            DrawBoard(gameState.Board);
            SetCursor(gameState.PlayerToMove);

            if (gameState.IsGameOver())
            {
                ShowGameOver();
            }
        }

        private void OnFromPositionSelected(Position position)
        {
            IEnumerable<Move> moves = gameState.LegalMoveForPiece(position);

            if (moves.Any())
            {
                selectedPosition = position;
                CacheMoves(moves);
                ShowHighlights();
            }
        }

        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);

            return new Position(row, column);
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();

            foreach (Move move in moves)
            {
                moveCache[move.ToPosition] = move;
            }
        }


        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position positionTo in moveCache.Keys)
            {
                highlights[positionTo.Row, positionTo.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position positionTo in moveCache.Keys)
            {
                highlights[positionTo.Row, positionTo.Column].Fill = Brushes.Transparent;
            }
        }

        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }

        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGameOver()
        {
            GameOverMenu gameOverMenu = new GameOverMenu(gameState);
            MenuContainer.Content = gameOverMenu;

            gameOverMenu.OptionSelected += option =>
            {
                if (option == Option.Restart)
                {
                    MenuContainer.Content = null;
                    RestartGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void RestartGame()
        {
            HideHighlights();
            moveCache.Clear();

            gameState = new GameState(Board.Initial(), Player.White);
            DrawBoard(gameState.Board);
            SetCursor(Player.White);
        }
    }
}
