using System.ComponentModel;
using Microsoft.Win32;
using System.Text.Json.Serialization;
using System.Text.Json;
using MvvmHelpers;

namespace quiz_maker.ViewModel
{
    public class QuizEditorViewModel : BaseViewModel
    {
        new public event PropertyChangedEventHandler? PropertyChanged;

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
        public RelayCommand SaveToJsonCommand { get; }
        public RelayCommand BackToMenuCommand { get; }

        public QuizEditorViewModel(Quiz quiz)
        {
            CurrentQuiz = quiz;
            AddQuestionCommand = new RelayCommand(AddQuestion);
            AddAnswerCommand = new RelayCommand(AddAnswer);
            SaveToJsonCommand = new RelayCommand(SaveToJson);
            BackToMenuCommand = new RelayCommand(BackToMenu);
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
            var q = new Question { Text = "Nowe pytanie" };
            CurrentQuiz.Questions.Add(q);
            SelectedQuestion = q;
        }

        private void AddAnswer()
        {
            SelectedQuestion?.Answers.Add(new Answer { Text = "Odpowiedź", IsCorrect = false });
        }

        new private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
