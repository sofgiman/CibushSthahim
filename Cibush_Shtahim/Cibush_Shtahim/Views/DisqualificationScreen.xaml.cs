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

namespace Cibush_Shtahim.Views
{
    /// <summary>
    /// Interaction logic for DisqualificationScreen.xaml
    /// </summary>
    public partial class DisqualificationScreen : UserControl
    {
        // points user earned
        private int points;
        public DisqualificationScreen()
        {
            InitializeComponent();
        }
        // when Disqualification screen loaded
        private void DisqualificationScreenControl_Loaded(object sender, RoutedEventArgs e)
        {
            // get user points and print them
            points = game.TotalPoints;
            Points.DataContext = points;
            // reset user points
            game.TotalPoints = 0;
        }

        private void ReturnToGameScreen_Click(object sender, RoutedEventArgs e)
        {
            //switch user control to game
            Window.GetWindow(this).DataContext = MainWindow.gameViewModel;           
        }

        private void ReturnToEntryScreen_Click(object sender, RoutedEventArgs e)
        {
            //switch user control to entry screen
            Window.GetWindow(this).DataContext = MainWindow.entryWindowViewModel;
        }


    }
}
