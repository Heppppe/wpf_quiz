public class QuizQuestion
{
    private List<QuizQuestion> _questions = new();
    private int _currentQuestionIndex = 0;
    public string QuestionText { get; set; }
    public List<string> Answers { get; set; }
}
