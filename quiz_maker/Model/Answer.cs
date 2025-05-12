using MvvmHelpers;

public class Answer : ObservableObject
{
    string _text;
    public string Text
    {
        get => _text;
        set => SetProperty(ref _text, value);
    }

    bool _isCorrect;
    public bool IsCorrect
    {
        get => _isCorrect;
        set => SetProperty(ref _isCorrect, value);
    }
}