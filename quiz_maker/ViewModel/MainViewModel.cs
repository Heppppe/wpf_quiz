using MvvmHelpers;
using System.Windows.Input;

namespace quiz_maker.ViewModel {
    public class MainViewModel : BaseViewModel
    {
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public ICommand NavigateToMenuCommand { get; }
        public ICommand NavigateToEditorCommand { get; }

        public MainViewModel()
        {
            NavigateToMenuCommand = new RelayCommand(_ => NavigateToMenu());
            NavigateToEditorCommand = new RelayCommand(q => NavigateToEditor(q));

            NavigateToMenu();
        }

        private void NavigateToMenu()
        {
            CurrentViewModel = new QuizMenuViewModel(this);
        }

        private void NavigateToEditor(Quiz quiz)
        {
            CurrentViewModel = new QuizEditorViewModel(quiz);
        }
    }
}