using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Solver_App.Model
{
    public class AnswerSystem : INotifyPropertyChanged
    {
        private ObservableCollection<Answer> _answers = new();
        public ObservableCollection<Answer> Answers
        {
            get => _answers;
            set
            {
                _answers = value;
                OnPropertyChanged(nameof(Answers));
            }
        }

        private List<int> _selectedAnswers = new List<int>();

        public List<int> SelectedAnswers
        {
            get => _selectedAnswers;
            set
            {
                if (_selectedAnswers != value)
                {
                    _selectedAnswers = value;
                    OnPropertyChanged(nameof(SelectedAnswers));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
