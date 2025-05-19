using System.Collections.ObjectModel;
using System.ComponentModel;

public class Question : INotifyPropertyChanged
{
    private string _text { get; set; } = "Wczytywanie pytania...";
    public string Text
    {
        get => _text;
        set { _text = value; OnPropertyChanged(nameof(Text)); }
    }

    public ObservableCollection<Answer> Answers { get; set; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}