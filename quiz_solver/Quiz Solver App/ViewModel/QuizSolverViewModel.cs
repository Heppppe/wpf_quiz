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
        private const int V = 1000;
        private readonly System.Threading.Timer _timer;
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

        public QuizSolverViewModel(int v)
        {
            // Przykładowe dane
            QuestionText = "Przykładowe pytanie?";
            Answers.Add("Odpowiedź 1");
            Answers.Add("Odpowiedź 2");
            Answers.Add("Odpowiedź 3");
            Answers.Add("Odpowiedź 4");

            //_timeLeft = 60;
            //_timer = new System.Threading.Timer(v);
            //_timer.Elapsed += (s, e) =>
            //{
            //    if (_timeLeft > 0)
            //    {
            //        _timeLeft--;
            //        OnPropertyChanged(nameof(TimeLeftDisplay));
            //    }
            //};
            //_timer.Start();

            NextQuestionCommand = new RelayCommand(_ => NextQuestion(), _ => true);
            PreviousQuestionCommand = new RelayCommand(_ => PreviousQuestion(), _ => true);
            SelectAnswerCommand = new RelayCommand(index => SelectedAnswerIndex = (int)index, _ => true);
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
