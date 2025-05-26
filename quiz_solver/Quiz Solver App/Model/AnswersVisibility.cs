using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Solver_App.Model
{
    public class AnswersVisibility : INotifyPropertyChanged
    {
        private bool _answer1Visible = true;
        private bool _answer2Visible = true;
        private bool _answer3Visible = true;
        private bool _answer4Visible = true;

        public bool Answer1Visible
        {
            get => _answer1Visible;
            set { _answer1Visible = value; OnPropertyChanged(nameof(Answer1Visible)); }
        }
        public bool Answer2Visible
        {
            get => _answer2Visible;
            set { _answer2Visible = value; OnPropertyChanged(nameof(Answer2Visible)); }
        }
        public bool Answer3Visible
        {
            get => _answer3Visible;
            set { _answer3Visible = value; OnPropertyChanged(nameof(Answer3Visible)); }
        }
        public bool Answer4Visible
        {
            get => _answer4Visible;
            set { _answer4Visible = value; OnPropertyChanged(nameof(Answer4Visible)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
