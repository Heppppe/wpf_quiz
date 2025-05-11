using System.Collections.ObjectModel;

public class Quiz
{
    public string Title { get; set; }
    public ObservableCollection<Question> Questions { get; set; } = new();
}