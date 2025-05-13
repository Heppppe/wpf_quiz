using System.ComponentModel;

public class Answer : INotifyPropertyChanged
{
    private string _text = "";
    private bool _isCorrect;

    public string Text
    {
        get => _text;
        set { _text = value; OnPropertyChanged(nameof(Text)); }
    }

    public bool IsCorrect
    {
        get => _isCorrect;
        set { _isCorrect = value; OnPropertyChanged(nameof(IsCorrect)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
