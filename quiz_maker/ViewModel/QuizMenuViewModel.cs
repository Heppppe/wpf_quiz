using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Printing.IndexedProperties;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;

namespace quiz_maker.ViewModel
{
    public class QuizMenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Quiz> Quizzes { get; set; } = new ObservableCollection<Quiz>();

        private Quiz _selectedQuiz;
        public Quiz SelectedQuiz
        {
            get => _selectedQuiz;
            set
            {
                _selectedQuiz = value;
                OnPropertyChanged();
            }
        }

        public ICommand EditQuizCommand { get; }

        public ICommand CreateNewQuizCommand { get; }
        public ICommand DeleteQuizCommand { get; }
        public QuizMenuViewModel()
        {
            LoadQuizzes();
            EditQuizCommand = new RelayCommand(EditQuiz, CanEditQuiz);
            CreateNewQuizCommand = new RelayCommand(CreateNewQuiz);
            DeleteQuizCommand = new RelayCommand(DeleteQuiz);
        }

        private void LoadQuizzes()
        {
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savedQuizes");

            if (!Directory.Exists(folderPath))
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Directory not found: {folderPath}");
                return;
            }

            foreach (var file in Directory.GetFiles(folderPath, "*.json"))
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var quiz = JsonSerializer.Deserialize<Quiz>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (quiz?.Questions != null)
                    {
                        Quizzes.Add(quiz);
                        System.Diagnostics.Debug.WriteLine($"[INFO] Loaded quiz: {quiz.Title}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[WARNING] Invalid quiz format in file: {Path.GetFileName(file)}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to load file: {Path.GetFileName(file)}");
                    System.Diagnostics.Debug.WriteLine($"        Reason: {ex.Message}");
                }
            }
        }
        private void DeleteQuiz(object parameter)
        {
            if (parameter is Quiz quiz)
            {
                Quizzes?.Remove(quiz);
            }
        }

        private void EditQuiz(object parameter)
        {
            if (parameter is Quiz quiz)
            {
                MainViewModel.Current.CurrentViewModel = new QuizEditorViewModel(quiz);
            }
        }

        private void CreateNewQuiz(object parameter)
        {
            var template = new Quiz
            {
                Title = "New Quiz",
                Questions =
                [
                    new Question {
                        Text = "New Question",
                        Answers =
                        [
                            new Answer { Text = "Answer 1", IsCorrect = false },
                            new Answer { Text = "Answer 2", IsCorrect = true }
                        ]
                    }
                ]
            };

            MainViewModel.Current.CurrentViewModel = new QuizEditorViewModel(template);
        }

        private bool CanEditQuiz(object parameter) => parameter is Quiz;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
