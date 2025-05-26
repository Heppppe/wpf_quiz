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
using Quiz_Solver_App.Model;

namespace Quiz_Solver_App.ViewModel
{
    public class MainMenuViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ICommand SelectFileCommand { get; }

        private readonly NavigationService _navigationService;
        private readonly ViewModelFactory _viewModelFactory;
        public ObservableCollection<QuizItems> Quizzes { get; set; } = new();


        private QuizItems _selectedQuiz;
        public QuizItems SelectedQuiz
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

        public MainMenuViewModel(NavigationService navigationService, ViewModelFactory viewModelFactory)
        {
            LoadQuizzes();

            _navigationService = navigationService;
            _viewModelFactory = viewModelFactory;
            SelectJsonFileCommand = new RelayCommand(
                    param =>
                    {
                        int quizLoadingMode = Convert.ToInt32(param);
                        NavigateToQuizSolver(quizLoadingMode, null);
                    },
                    _ => true
                );
            ChooseCommand = new RelayCommand(
                param =>
                    {
                        if (param is QuizItems quizItem)
                        {
                            SelectedQuiz = quizItem;
                            SelectedJsonFilePath = quizItem.FilePath;

                            var quiz = quizItem.Quiz;

                            NavigateToQuizSolver(0,SelectedJsonFilePath);
                        }
                    },
                    _ => true
                );
        }

        private void NavigateToQuizSolver(int quizLoadingMode = 0, string fullPath = null)
        {
            _navigationService.NavigateTo(_viewModelFactory.CreateQuizSolverVM(quizLoadingMode, fullPath));
        }


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
                var quiz = AES.DecryptQuiz(file);

                if (quiz?.Questions != null)
                {
                    var quizVm = new QuizItems(quiz, Path.GetFullPath(file));
                    Quizzes.Add(quizVm);
                }
                else
                {
                    MessageBox.Show($"[WARNING] Invalid quiz format in file: {Path.GetFileName(file)}", "Error");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

