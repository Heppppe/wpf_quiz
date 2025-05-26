using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Solver_App.Model
{
    public static class PointsSystem
    {
        public static int CalculatePoints(Quiz quiz)
        {
            if (quiz == null || quiz.Questions == null)
            {
                return -1;
            }

            int points = 0;
            foreach (var question in quiz.Questions)
            {
                int pointsForQuestion = 0;

                for (int i = 0; i < question.Answers.Count; i++)
                {
                    if (question.SelectedAnswers.Contains(i) && question.Answers[i].IsCorrect)
                    {
                        pointsForQuestion++;
                    }
                    else if (question.SelectedAnswers.Contains(i) && !question.Answers[i].IsCorrect)
                    {
                        pointsForQuestion--;
                    }
                }

                if (pointsForQuestion > 0)
                {
                    points += pointsForQuestion;
                }
                else if (pointsForQuestion <= 0)
                {
                    pointsForQuestion = 0;
                    points += pointsForQuestion;
                }
            }
            return points;
        }

        public static int CalculateMaxPoints(Quiz quiz)
        {
            if (quiz == null || quiz.Questions == null)
            {
                return -1;
            }

            int maxPoints = 0;
            foreach (var question in quiz.Questions)
            {
                int maxPointsForQuestion = 0;
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    if (question.Answers[i].IsCorrect)
                    {
                        maxPointsForQuestion++;
                    }
                }
                maxPoints += maxPointsForQuestion;
            }
            return maxPoints;
        }
    }
}
