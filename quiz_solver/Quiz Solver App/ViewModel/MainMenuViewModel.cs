using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Quiz_Solver_App.Services;
using Quiz_Solver_App.ViewModel.Base;
using Microsoft.Win32;
using System.Windows.Input;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Quiz_Solver_App.ViewModel
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly ViewModelFactory _viewModelFactory;
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
        public ICommand SelectJsonFileCommand { get; }
        public ICommand ChooseCommand { get; }
        private string _selectedJsonFilePath;
        public string SelectedJsonFilePath
        {
            get => _selectedJsonFilePath;
            set
            {
                _selectedJsonFilePath = value;
                OnPropertyChanged(nameof(SelectedJsonFilePath));
            }
        }

        public MainMenuViewModel()
        {
            LoadQuizzes();
            //_navigationService = navigationService;
            //_viewModelFactory = viewModelFactory;

            SelectJsonFileCommand = new RelayCommand(_ => SelectJsonFile(), _ => true);
            ChooseCommand = new RelayCommand(_ => ChooseQuiz(SelectedQuiz), _ => true);
        }
        private static readonly byte[] aesKey = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
        private static readonly byte[] aesIV = Encoding.UTF8.GetBytes("ABCDEF0123456789");
        private void LoadQuizzes()
        {
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../../savedQuizes");

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
        private void ChooseQuiz(object parameter)
        {
            if (parameter is Quiz quiz)
            {
                MainViewModel.Current.CurrentViewModel = new QuizSolverViewModel(100);
            }
        }
        private void SelectJsonFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki JSON (*.json)|*.json",
                Title = "Wybierz plik quizu"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedJsonFilePath = openFileDialog.FileName;
                // Tutaj możesz dodać kod do wczytania i przetworzenia pliku
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

