using MvvmHelpers;
using System.ComponentModel;
using System.Windows.Input;

namespace quiz_maker.ViewModel {
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
            CurrentViewModel = new QuizMenuViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}