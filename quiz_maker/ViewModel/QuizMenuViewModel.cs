using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Printing.IndexedProperties;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
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

        private static readonly byte[] aesKey = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
        private static readonly byte[] aesIV = Encoding.UTF8.GetBytes("ABCDEF0123456789");
        private void LoadQuizzes()
        {
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../savedQuizes");

            if (!Directory.Exists(folderPath))
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Directory not found: {folderPath}");
                return;
            }

            foreach (var file in Directory.GetFiles(folderPath, "*.json"))
            {
                try
                {
                    byte[] encryptedData = File.ReadAllBytes(file);

                    string json = DecryptStringFromBytes_Aes(encryptedData, aesKey, aesIV);

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
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new(cipherText);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
        private void DeleteQuiz(object parameter)
        {
            if (parameter is Quiz quiz)
            {
                Quizzes?.Remove(quiz);

                try
                {
                    var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../savedQuizes");
                    var deletedPath = Path.Combine(basePath, "deletedQuizes");

                    if (!Directory.Exists(deletedPath))
                    {
                        Directory.CreateDirectory(deletedPath);
                    }

                    string safeTitle = string.Concat(quiz.Title.Split(Path.GetInvalidFileNameChars()));
                    string sourceFile = Path.Combine(basePath, $"{safeTitle}.json");
                    string destFile = Path.Combine(deletedPath, $"{safeTitle}.json");

                    if (File.Exists(sourceFile))
                    {
                        File.Move(sourceFile, destFile);
                        System.Diagnostics.Debug.WriteLine($"[INFO] Moved quiz file to deletedQuizes: {quiz.Title}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[WARNING] Quiz file not found: {sourceFile}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to move quiz file: {ex.Message}");
                }
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
