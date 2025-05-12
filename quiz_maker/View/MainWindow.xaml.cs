using System.Windows;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;
using quiz_maker.ViewModel;

namespace quiz_maker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(); // Set the DataContext here
        }
    }
}