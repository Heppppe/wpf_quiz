using System.Collections.ObjectModel;
using System.ComponentModel;
using Quiz_Solver_App.Model;

public class Question : INotifyPropertyChanged
{
    private string _text;
    public string Text
    {
        get => _text;
        set { _text = value; OnPropertyChanged(nameof(Text)); }
    }

    public ObservableCollection<Answer> Answers { get; set; }

    public AnswersVisibility AnswersVisibility { get; set; }

    private List<int> _selectedAnswers = new List<int>() { 0, 1, 2, 3 };

    public List<int> SelectedAnswers
    {
        get => _selectedAnswers;
        set
        {
            if (_selectedAnswers != value)
            {
                _selectedAnswers = value;
                OnPropertyChanged(nameof(SelectedAnswers));
            }
        }
    }

    public Question(string text)
    {
        Text = text;
        Answers = new ObservableCollection<Answer>();
        AnswersVisibility = new AnswersVisibility();
    }

    private bool _selectedAnswer1;
    public bool SelectedAnswer1
    {
        get => _selectedAnswer1;
        set 
        {
            _selectedAnswer1 = value;
            OnPropertyChanged(nameof(SelectedAnswer1));
            UpdateIsMultipleChoice(0);
        }
    }
    private bool _selectedAnswer2;
    public bool SelectedAnswer2
    {
        get => _selectedAnswer2;
        set 
        {
            _selectedAnswer2 = value;
            OnPropertyChanged(nameof(SelectedAnswer2));
            UpdateIsMultipleChoice(1);
        }
    }
    private bool _selectedAnswer3;
    public bool SelectedAnswer3
    {
        get => _selectedAnswer3;
        set
        {
            _selectedAnswer3 = value;
            OnPropertyChanged(nameof(SelectedAnswer3));
            UpdateIsMultipleChoice(2);
        }
    }
    private bool _selectedAnswer4;
    public bool SelectedAnswer4
    {
        get => _selectedAnswer4;
        set
        {
            _selectedAnswer4 = value;
            OnPropertyChanged(nameof(SelectedAnswer4));
            UpdateIsMultipleChoice(3);
        }
    }

    public void AddAnswer(string text, bool isCorrect)
    {
        Answers.Add(new Answer(text, isCorrect));
    }

    public void RemoveAnswer(Answer answer)
    {
        Answers.Remove(answer);
    }

    private bool[] _isAnswerVisible = new bool[] { false, false, false, false };
    public bool[] IsAnswerVisible
    {
        get => _isAnswerVisible;
        set
        {
            _isAnswerVisible = value;
            OnPropertyChanged(nameof(IsAnswerVisible));
        }
    }

    public void UpdateIsMultipleChoice(int answerNumber)
    {
        if (SelectedAnswers.Contains(answerNumber))
            SelectedAnswers.Remove(answerNumber);
        else
            SelectedAnswers.Add(answerNumber);

        OnPropertyChanged(nameof(SelectedAnswers));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
