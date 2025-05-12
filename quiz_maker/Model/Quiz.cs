using System.Collections.ObjectModel;
using System.ComponentModel;

public class Quiz : INotifyPropertyChanged
{
    public string _title { get; set; } = "Nowy Quiz";
    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(nameof(Title)); }
    }
    public ObservableCollection<Question> Questions { get; set; } = new();
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}