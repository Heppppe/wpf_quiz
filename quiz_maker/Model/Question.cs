using System.Collections.ObjectModel;

public class Question
{
    public string Text { get; set; }
    public ObservableCollection<Answer> Answers { get; set; } = new();
}