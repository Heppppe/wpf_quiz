using System.ComponentModel;
using Microsoft.Win32;
using System.Text.Json.Serialization;
using System.Text.Json;
using MvvmHelpers;
using System.Reflection.Metadata;

namespace quiz_maker.ViewModel
{
    public class QuizEditorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Quiz CurrentQuiz { get; set; } = new() { Title = "Nowy Quiz" };

        private Question? _selectedQuestion;
        public Question? SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged(nameof(SelectedQuestion));
                AddAnswerCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand AddQuestionCommand { get; }
        public RelayCommand AddAnswerCommand { get; }
        public RelayCommand DeleteAnswerCommand { get; }
        public RelayCommand SaveToJsonCommand { get; }
        public RelayCommand BackToMenuCommand { get; }

        public QuizEditorViewModel(Quiz quiz)
        {
            CurrentQuiz = quiz;
            AddQuestionCommand = new RelayCommand(AddQuestion);
            AddAnswerCommand = new RelayCommand(AddAnswer);
            SaveToJsonCommand = new RelayCommand(SaveToJson);
            BackToMenuCommand = new RelayCommand(BackToMenu);
            DeleteAnswerCommand = new RelayCommand(DeleteAnswer);
        }

        private void SaveToJson()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                FileName = $"{CurrentQuiz.Title}.json"
            };

            if (dialog.ShowDialog() == true)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string json = JsonSerializer.Serialize(CurrentQuiz, options);
                // File.WriteAllText(dialog.FileName, json);
            }
        }
        private void BackToMenu()
        {
            MainViewModel.Current.CurrentViewModel = new QuizMenuViewModel();
        }

        private void AddQuestion()
        {
            var q = new Question { Text = "", Answers = [ new Answer { Text="", IsCorrect=false } ] };
            CurrentQuiz.Questions.Add(q);
            SelectedQuestion = q;
        }

        private void AddAnswer()
        {
            SelectedQuestion?.Answers.Add(new Answer { Text = "Nowa odpowiedź", IsCorrect = false });
        }

        private void DeleteAnswer(object parameter)
        {
            if (parameter is Answer answer)
            {
                SelectedQuestion?.Answers?.Remove(answer);
            }
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
