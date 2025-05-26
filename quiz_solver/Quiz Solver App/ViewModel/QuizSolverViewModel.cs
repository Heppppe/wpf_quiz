using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using Quiz_Solver_App.Services;
using Quiz_Solver_App.ViewModel.Base;
using System.Timers;
using Quiz_Solver_App.Model;
using System.Text.Json;
using System.Text;

namespace Quiz_Solver_App.ViewModel
{
    public class QuizSolverViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly DispatcherTimer _timer;
        private int _currentIndex;
        public string QuizResultText => IsQuizCompleted ? CalculateResult() : string.Empty;

        public ICommand LoadQuizCommand { get; }

        private Quiz _selectedQuizSolverQuiz;
        public Quiz SelectedQuizSolverQuiz
        {
            get => _selectedQuizSolverQuiz;
            set
            {
                _selectedQuizSolverQuiz = value;
                OnPropertyChanged();
            }
        }

        private bool _isQuizActive;
        public bool IsQuizActive
        {
            get => _isQuizActive;
            private set
            {
                if (_isQuizActive != value)
                {
                    _isQuizActive = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isQuizLoaded = false;
        public bool IsQuizLoaded
        {
            get => _isQuizLoaded;
            set
            {
                _isQuizLoaded = value;
                OnPropertyChanged(nameof(IsQuizLoaded));
            }
        }

        private bool _canLoadQuiz = true;
        public bool CanLoadQuiz
        {
            get => _canLoadQuiz;
            set
            {
                _canLoadQuiz = value;
                OnPropertyChanged(nameof(CanLoadQuiz));
            }
        }

        private bool _isQuizCompleted;
        public bool IsQuizCompleted
        {
            get => _isQuizCompleted;
            set
            {
                _isQuizCompleted = value;
                OnPropertyChanged(nameof(IsQuizCompleted));
            }
        }

        public ObservableCollection<Answer> Answers { get; } = new();
        private string _questionText;
        public string QuestionText
        {
            get => _questionText;
            set { _questionText = value; OnPropertyChanged(nameof(QuestionText)); }
        }

        private int _selectedAnswerIndex = -1;
        public int SelectedAnswerIndex
        {
            get => _selectedAnswerIndex;
            set { _selectedAnswerIndex = value; OnPropertyChanged(nameof(SelectedAnswerIndex)); }
        }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                QuestionText = _currentQuestion.Text;
                Answers.Clear();
                int i = 0;
                foreach (var answer in _currentQuestion.Answers)
                {
                    Answers.Add(answer);
                    if (_currentQuestion.Answers.IndexOf(answer) == i)
                    {
                        _currentQuestion.IsAnswerVisible[i] = true;
                    }
                    else
                    {
                        _currentQuestion.IsAnswerVisible[i] = false;
                    }
                    i++;
                }
                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(QuestionText));
                UpdateAnswers();

                if (IsQuizCompleted == true)
                {
                    ShowCorrectAnswers();
                }
            }
        }

        public string CurrentQuestionText => _currentQuestion == null
            ? "Brak dostępnych pytań"
            : $"Pytanie #{_currentIndex + 1}: {_currentQuestion.Text}";

        private TimeSpan _elapsedTime;
        public string TimeLeftDisplay => _elapsedTime.ToString(@"mm\:ss");

        public ICommand NextQuestionCommand { get; }
        public ICommand PreviousQuestionCommand { get; }
        public ICommand SelectAnswerCommand { get; }
        public ICommand NavigateMainMenuCommand { get; }

        public QuizSolverViewModel(NavigationService navigationService, ViewModelFactory viewModelFactory, int quizLoadingMode, string fullPath)
        {
            _navigationService = navigationService;
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += TimerTick;

            if (quizLoadingMode == 0)
            {
                _selectedQuizSolverQuiz = QuizLoading.LoadQuizFromFile(fullPath);
                OnPropertyChanged(nameof(SelectedQuizSolverQuiz));
                InitiateQuiz();
            }
            else if (quizLoadingMode == 1)
            {
                LoadQuiz();
            }

            NextQuestionCommand = new RelayCommand(_ => NextQuestion(), _ => NextQuestionCondition());
            PreviousQuestionCommand = new RelayCommand(_ => PreviousQuestion(), _ => PrevQuestionCondition());
            LoadQuizCommand = new RelayCommand(_ => LoadQuiz(), _ => CanLoadQuiz);
            NavigateMainMenuCommand = new RelayCommand(_ => ConfirmExit(viewModelFactory), _ => true);
        }

        private bool NextQuestionCondition()
        {
            if (SelectedQuizSolverQuiz == null)
            {
                return false;
            }
            return SelectedQuizSolverQuiz.Questions != null && _currentIndex < SelectedQuizSolverQuiz.Questions.Count - 1;
        }

        private bool PrevQuestionCondition()
        {
            if (SelectedQuizSolverQuiz == null)
            {
                return false;
            }
            return SelectedQuizSolverQuiz.Questions != null && _currentIndex > 0;
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (_isQuizActive)
            {
                _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
                OnPropertyChanged(nameof(TimeLeftDisplay));
            }
        }

        private void ConfirmExit(ViewModelFactory viewModelFactory)
        {
            string message = "Czy na pewno chcesz wrócić do głównego menu?";
            MessageBoxResult result = MessageBox.Show(message, "Potwierdzenie zakończenia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _navigationService.NavigateTo(viewModelFactory.CreateMainMenuVM());
            }
        }

        private void LoadQuiz()
        {
            IsQuizLoaded = false;
            _selectedQuizSolverQuiz = QuizLoading.LoadQuizManually();
            if (SelectedQuizSolverQuiz == null)
            {
                MessageBox.Show("Nie wybrano quizu!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                CanLoadQuiz = true;
                return;
            }
            else
            {
                ClearQuiz();
                InitiateQuiz();
                OnPropertyChanged(nameof(SelectedQuizSolverQuiz));
            }
        }

        private void ClearQuiz()
        {
            _currentIndex = 0;
            QuestionText = string.Empty;
            Answers.Clear();
            IsQuizActive = false;
            IsQuizCompleted = false;
            CanLoadQuiz = true;
            _currentIndex = 0;
            IsAnswer1Correct = null;
            IsAnswer2Correct = null;
            IsAnswer3Correct = null;
            IsAnswer4Correct = null;
            OnPropertyChanged(nameof(CurrentQuestion));
            OnPropertyChanged(nameof(QuizTitle));
            OnPropertyChanged(nameof(QuestionText));
            OnPropertyChanged(nameof(Answer1Text));
            OnPropertyChanged(nameof(Answer2Text));
            OnPropertyChanged(nameof(Answer3Text));
            OnPropertyChanged(nameof(Answer4Text));
            OnPropertyChanged(nameof(QuizResultText));

        }

        private void NextQuestion()
        {

            if (_currentIndex < SelectedQuizSolverQuiz.Questions.Count - 1)
            {
                _currentIndex++;
                CurrentQuestion = SelectedQuizSolverQuiz.Questions[_currentIndex];
                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(CurrentQuestionText));

            }
            else
            {
                EndQuiz();
            }

        }


        private void PreviousQuestion()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                CurrentQuestion = SelectedQuizSolverQuiz.Questions[_currentIndex];
                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(CurrentQuestionText));
            }
        }

        public string Answer1Text => CurrentQuestion == null ? string.Empty : (CurrentQuestion.Answers.Count > 0 ? CurrentQuestion.Answers[0].Text : string.Empty);
        public string Answer2Text => CurrentQuestion == null ? string.Empty : (CurrentQuestion.Answers.Count > 1 ? CurrentQuestion.Answers[1].Text : string.Empty);
        public string Answer3Text => CurrentQuestion == null ? string.Empty : (CurrentQuestion.Answers.Count > 2 ? CurrentQuestion.Answers[2].Text : string.Empty);
        public string Answer4Text => CurrentQuestion == null ? string.Empty : (CurrentQuestion.Answers.Count > 3 ? CurrentQuestion.Answers[3].Text : string.Empty);


        private ICommand _endQuizCommand;
        public ICommand EndQuizCommand
        {
            get
            {
                return _endQuizCommand ??= new RelayCommand(_ =>
                {
                    ConfirmEndQuiz();
                }, _ => IsQuizLoaded);
            }
        }

        private void ConfirmEndQuiz()
        {
            string message = $"Czy na pewno chcesz zakończyć quiz?{Environment.NewLine}Nie będzie można go wznowić.";
            MessageBoxResult result = MessageBox.Show(message, "Potwierdzenie zakończenia", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                EndQuiz();
            }
        }

        public bool IsAnswer1Visible => GetAnswerVisible(0);
        public bool IsAnswer2Visible => GetAnswerVisible(1);
        public bool IsAnswer3Visible => GetAnswerVisible(2);
        public bool IsAnswer4Visible => GetAnswerVisible(3);

        private void EndQuiz()
        {
            AreCheckBoxesEnabled = false;
            IsQuizActive = false;
            IsQuizCompleted = true;
            IsQuizLoaded = false;
            _timer.Stop();
            ShowCorrectAnswers();
            OnPropertyChanged(nameof(QuizResultText));
            MessageBox.Show(QuizResultText, "Wynik", MessageBoxButton.OK, MessageBoxImage.Information);
            CanLoadQuiz = true;
        }

        public string QuizTitle
        {
            get => SelectedQuizSolverQuiz == null ? "Brak quizu do wyświetlenia": $"Nazwa quizu: {SelectedQuizSolverQuiz.Title}";
        }

        private void InitiateQuiz()
        {
            string message = $"Quiz \"{SelectedQuizSolverQuiz.Title}\" został pomyślnie wczytany!\n" +
                     $"Liczba pytań: {SelectedQuizSolverQuiz.Questions.Count}\n\n" +
                     $"Czy jesteś gotowy(a), aby rozpocząć quiz?";
            MessageBoxResult result = MessageBox.Show(message, "Potwierdzenie rozpoczęcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                _selectedQuizSolverQuiz = null;
                CanLoadQuiz = true;
                return;
            }

            IsQuizLoaded = true;
            _elapsedTime = TimeSpan.Zero;
            CanLoadQuiz = false;
            IsQuizActive = true;
            IsQuizCompleted = false;
            _currentIndex = 0;

            foreach (var question in SelectedQuizSolverQuiz.Questions)
            {
                question.SelectedAnswer1 = false;
                question.SelectedAnswer2 = false;
                question.SelectedAnswer3 = false;
                question.SelectedAnswer4 = false;
            }

            CurrentQuestion = SelectedQuizSolverQuiz.Questions[_currentIndex];
            QuestionText = CurrentQuestion.Text;

            _timer.Start();
            AreCheckBoxesEnabled = true;
            OnPropertyChanged(nameof(CurrentQuestion));
            OnPropertyChanged(nameof(TimeLeftDisplay));
            OnPropertyChanged(nameof(CurrentQuestionText));
            OnPropertyChanged(nameof(QuizTitle));
        }

        private void UpdateAnswers()
        {
            OnPropertyChanged(nameof(Answer1Text));
            OnPropertyChanged(nameof(Answer2Text));
            OnPropertyChanged(nameof(Answer3Text));
            OnPropertyChanged(nameof(Answer4Text));

            if (_currentQuestion.IsAnswerVisible[0]) _currentQuestion.AnswersVisibility.Answer1Visible = true;
            else _currentQuestion.AnswersVisibility.Answer1Visible = false;

            if (_currentQuestion.IsAnswerVisible[1]) _currentQuestion.AnswersVisibility.Answer2Visible = true;
            else _currentQuestion.AnswersVisibility.Answer2Visible = false;

            if (_currentQuestion.IsAnswerVisible[2]) _currentQuestion.AnswersVisibility.Answer3Visible = true;
            else _currentQuestion.AnswersVisibility.Answer3Visible = false;

            if (_currentQuestion.IsAnswerVisible[3]) _currentQuestion.AnswersVisibility.Answer4Visible = true;
            else _currentQuestion.AnswersVisibility.Answer4Visible = false;
        }
        public bool GetAnswerVisible(int index)
        {
            return CurrentQuestion?.IsAnswerVisible != null &&
                   index >= 0 &&
                   index < CurrentQuestion.Answers.Count
                ? CurrentQuestion.IsAnswerVisible[index]
                : false;
        }

        private bool _areCheckBoxesEnabled = true;
        public bool AreCheckBoxesEnabled
        {
            get => _areCheckBoxesEnabled;
            set
            {
                _areCheckBoxesEnabled = value;
                OnPropertyChanged(nameof(AreCheckBoxesEnabled));
            }
        }

        private bool? _isAnswer1Correct;
        public bool? IsAnswer1Correct
        {
            get => _isAnswer1Correct;
            set
            {
                _isAnswer1Correct = value;
                OnPropertyChanged(nameof(IsAnswer1Correct));
            }
        }

        private bool? _isAnswer2Correct;
        public bool? IsAnswer2Correct
        {
            get => _isAnswer2Correct;
            set
            {
                _isAnswer2Correct = value;
                OnPropertyChanged(nameof(IsAnswer2Correct));
            }
        }

        private bool? _isAnswer3Correct;
        public bool? IsAnswer3Correct
        {
            get => _isAnswer3Correct;
            set
            {
                _isAnswer3Correct = value;
                OnPropertyChanged(nameof(IsAnswer3Correct));
            }
        }

        private bool? _isAnswer4Correct;
        public bool? IsAnswer4Correct
        {
            get => _isAnswer4Correct;
            set
            {
                _isAnswer4Correct = value;
                OnPropertyChanged(nameof(IsAnswer4Correct));
            }
        }

        private void ShowCorrectAnswers()
        {
            bool?[] CorrectAnswers = {null,null,null,null};
            for (int i = 0; i < CurrentQuestion.Answers.Count; i++)
            {
                CorrectAnswers[i] = CurrentQuestion.Answers[i].IsCorrect;
            }
            IsAnswer1Correct = CorrectAnswers[0];
            IsAnswer2Correct = CorrectAnswers[1];
            IsAnswer3Correct = CorrectAnswers[2];
            IsAnswer4Correct = CorrectAnswers[3];
        }

        private string CalculateResult()
        {
            int score = 0;
            int maxPoints = 0;
            score = PointsSystem.CalculatePoints(_selectedQuizSolverQuiz);
            maxPoints = PointsSystem.CalculateMaxPoints(_selectedQuizSolverQuiz);

            return ($"Quiz zakończony!{Environment.NewLine}Twój wynik:{Environment.NewLine}{score}/{maxPoints} ({(score / (double)maxPoints * 100):F2}%) {Environment.NewLine}Czas: {TimeLeftDisplay}");
        }
    }
}
