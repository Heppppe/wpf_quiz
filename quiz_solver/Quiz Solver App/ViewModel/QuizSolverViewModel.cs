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
        private readonly DispatcherTimer _timer;
        private int _timeLeft;

        public ObservableCollection<string> Answers { get; } = new();
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

        public string TimeLeftDisplay => TimeSpan.FromSeconds(_timeLeft).ToString(@"mm\:ss");

        public ICommand NextQuestionCommand { get; }
        public ICommand PreviousQuestionCommand { get; }
        public ICommand SelectAnswerCommand { get; }

        public QuizSolverViewModel()
        {
            // Przykładowe dane
            QuestionText = "Przykładowe pytanie?";
            Answers.Add("Odpowiedź 1");
            Answers.Add("Odpowiedź 2");
            Answers.Add("Odpowiedź 3");
            Answers.Add("Odpowiedź 4");

            _timeLeft = 60;

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
            if (_timeLeft > 0)
            {
                _timeLeft--;
                OnPropertyChanged(nameof(TimeLeftDisplay));
            }
            else
            {
                _timer.Stop();
                // Możesz dodać logikę po zakończeniu czasu
            }
        }

        private void NextQuestion()
        {
            // Logika przejścia do następnego pytania
        }

        private void PreviousQuestion()
        {
            // Logika przejścia do poprzedniego pytania
        }
    }
}
