using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Quiz_Solver_App.Services;
using Quiz_Solver_App.ViewModel.Base;

namespace Quiz_Solver_App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }

        public MainViewModel()
        {
        }
    }

}
