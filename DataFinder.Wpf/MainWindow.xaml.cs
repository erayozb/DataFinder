using System.Windows;
using DataFinder.Wpf.ViewModels;

namespace DataFinder.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}