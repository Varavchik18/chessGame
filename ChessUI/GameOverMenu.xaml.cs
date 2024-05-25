using ChessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;
        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;
            WinnerText.Text = GetWinnerName(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, gameState.PlayerToMove);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Restart);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Exit);
        }

        private static string GetWinnerName(Player winner)
        {
            return winner switch
            {
                Player.White => "WHITE WINS",
                Player.Black => "BLACK WINS!",
                _ => "IT'S A DRAW"
            };

        }

        private static string PlayerString(Player player)
        {
            return player switch { 
                Player.White => "WHITE", 
                Player.Black => "BLACK", 
                _ => "" };
        }

        private static string GetReasonText(ReasonOfTheEnd reason, Player currentPlayer)
        {
            return reason switch
            {
                ReasonOfTheEnd.Stalemate => $"STALEMATE - {PlayerString(currentPlayer)} CAN'T MOVE",
                ReasonOfTheEnd.Checkmate=> $"CHECKMATE - {PlayerString(currentPlayer)} CAN'T MOVE",
                ReasonOfTheEnd.ThreeFoldRepetition=> $"THREE FOLD REPETITION",
                ReasonOfTheEnd.FiftyMoveRole=> $"FIFTY MOVE RULE",
                ReasonOfTheEnd.InsufficientMaterial=> $"INSUFFICIENT MATERIAL",
            };
        }
    }
}
