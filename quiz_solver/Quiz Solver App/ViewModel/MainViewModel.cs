using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Quiz_Solver_App.Services;
using Quiz_Solver_App.ViewModel.Base;

namespace Quiz_Solver_App.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }
        public static MainViewModel Current { get; private set; }

        public MainViewModel()
        {
            Current = this;
            CurrentViewModel = new MainMenuViewModel();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
