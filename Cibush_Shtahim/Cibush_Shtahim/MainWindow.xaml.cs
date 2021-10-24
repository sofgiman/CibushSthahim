using Cibush_Shtahim.ViewModels;
using Cibush_Shtahim.Views;
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


namespace Cibush_Shtahim
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
   
    public partial class MainWindow : Window
    {
       // the 3 screen of the game
       public static  GameViewModel gameViewModel = new GameViewModel();
       public static EntryWindowViewModel entryWindowViewModel = new EntryWindowViewModel();
       public static DisqualificationScreen disqualificationScreen = new DisqualificationScreen();


        public MainWindow()
        {
            InitializeComponent();
            
         
            // load entry screen view model to main window content control content
            DataContext = entryWindowViewModel;          
        }
          
    
    }
}
