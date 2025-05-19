using System.Configuration;
using System.Data;
using System.Windows;
using Quiz_Solver_App.ViewModel;

namespace Quiz_Solver_App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    var navigationService = new Quiz_Solver_App.Services.NavigationService();
        //    var viewModelFactory = new Quiz_Solver_App.ViewModel.Base.ViewModelFactory(navigationService);

        //    var mainViewModel = new MainViewModel();

        //    navigationService.SetNavigator(vm => mainViewModel.CurrentViewModel = vm);
        //    //mainViewModel.CurrentViewModel = viewModelFactory.CreateMainMenuVM();

        //    var mainWindow = new MainWindow
        //    {
        //        DataContext = mainViewModel
        //    };

        //    mainWindow.Show();
        //}
    }

}
