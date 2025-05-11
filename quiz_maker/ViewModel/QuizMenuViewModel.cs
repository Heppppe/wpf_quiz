using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;

namespace quiz_maker.ViewModel
{
    public class QuizMenuViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public ObservableCollection<Quiz> Quizzes { get; set; }

        public ICommand EditQuizCommand { get; }

        public QuizMenuViewModel(MainViewModel mainVM)
        {
            _mainViewModel = mainVM;
            //Quizzes = LoadQuizzes(); 
            //EditQuizCommand = new RelayCommand(q => _mainViewModel.NavigateToEditorCommand.Execute(q));
        }
    }

}
