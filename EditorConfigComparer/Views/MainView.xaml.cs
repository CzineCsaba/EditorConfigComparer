using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using EditorConfigComparer.ViewModels;

namespace EditorConfigComparer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }

        public MainViewModel ViewModel { get; }

        private void LoadLeftFile(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadLeftFile();
        }

        private void LoadRightFile(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadRightFile();
        }

        private void SelectAllRulesOnTheLeft(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectAllRulesOnTheLeft();
        }

        private void SelectAllRulesOnTheRight(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectAllRulesOnTheRight();
        }
    }
}