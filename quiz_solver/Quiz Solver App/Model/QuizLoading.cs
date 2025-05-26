using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Quiz_Solver_App.Model
{
   public static class QuizLoading
    {
        public static Quiz LoadQuizFromFile(string filePath)
        {
            var quiz = AES.DecryptQuiz(filePath);
            return quiz;
        }

        public static Quiz LoadQuizManually()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki JSON (*.json)|*.json",
                DefaultExt = ".json",
                Title = "Wybierz plik quizu"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string manuallyChosenPath = openFileDialog.FileName;

                Quiz manuallySelectedQuiz = LoadQuizFromFile(manuallyChosenPath);
                return manuallySelectedQuiz;
            }
            else
            {
                return null;
            }
        }
    }
}
