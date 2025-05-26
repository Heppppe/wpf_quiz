using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Solver_App.Model
{
    public class QuizItems
    {
        public Quiz Quiz { get; }

        public string FilePath { get; }

        public string Title => Quiz.Title;

        public QuizItems(Quiz quiz, string filePath)
        {
            Quiz = quiz;
            FilePath = filePath;
        }
    }
}
