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

namespace Quiz_Solver_App.ViewModel
{
    public class QuizSolverViewModel : ViewModelBase
    {
        private bool isQuizActive;
        private Quiz _quiz;
        private readonly DispatcherTimer _timer;
        private int _timeLeft;


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
                foreach (var answer in _currentQuestion.Answers)
                {
                    Answers.Add(answer);
                }
                OnPropertyChanged(nameof(CurrentQuestion));
            }
        }

        public string TimeLeftDisplay => TimeSpan.FromSeconds(_timeLeft).ToString(@"mm\:ss");

        public ICommand NextQuestionCommand { get; }
        public ICommand PreviousQuestionCommand { get; }
        public ICommand SelectAnswerCommand { get; }

        public QuizSolverViewModel()
        {
            // Przykładowe dane
            QuestionText = "Przykładowe pytanie?";
            //Answers.Add("Odpowiedź 1");
            //Answers.Add("Odpowiedź 2");
            //Answers.Add("Odpowiedź 3");
            //Answers.Add("Odpowiedź 4");

            _timeLeft = 0;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            NextQuestionCommand = new RelayCommand(_ => NextQuestion(), _ => true);
            PreviousQuestionCommand = new RelayCommand(_ => PreviousQuestion(), _ => true);
            SelectAnswerCommand = new RelayCommand(index => SelectedAnswerIndex = (int)index, _ => true);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
                _timeLeft++;
                OnPropertyChanged(nameof(TimeLeftDisplay));
        }

        private void NextQuestion()
        {
            CurrentQuestion = _quiz.Questions[_quiz.Questions.IndexOf(CurrentQuestion) + 1];
            OnPropertyChanged(nameof(CurrentQuestion));

        }

        private void PreviousQuestion()
        {
            CurrentQuestion = _quiz.Questions[_quiz.Questions.IndexOf(CurrentQuestion) - 1];
            OnPropertyChanged(nameof(CurrentQuestion));
        }

        private ICommand _endQuizCommand;
        public ICommand EndQuizCommand
        {
            get
            {
                return _endQuizCommand ??= new RelayCommand(_ =>
                {
                    EndQuiz();
                    MessageBox.Show("Quiz zakończony!");
                }, _ => true);
            }
        }

        private void EndQuiz()
        {
            isQuizActive = false;
            _timer.Stop();
        }
    }
}
