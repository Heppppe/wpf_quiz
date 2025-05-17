using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace quiz_maker.ViewModel{
    public class CreateQuiz : INotifyPropertyChanged
    {
        private string _quizName;

        public string QuizName
        {
            get => _quizName;
            set { _quizName = value; OnPropertyChanged(); }
        }

        public ICommand ShowNameCommand { get; }

        //public CreateQuiz()
        //{
        //    ShowNameCommand = new RelayCommand(ShowName);
        //}
        private void ShowName()
        {
            QuizName = "Nowy Quiz";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}