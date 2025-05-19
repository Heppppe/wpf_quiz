using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz_Solver_App.Services;
using Quiz_Solver_App.View;

namespace Quiz_Solver_App.ViewModel.Base
{
    public class ViewModelFactory
    {
        private readonly NavigationService _navigationService;

        public ViewModelFactory(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        //public MainMenuViewModel CreateMainMenuVM()
        //{
        //    return new MainMenuViewModel(_navigationService, this);
        //}

        //public QuizSolverViewModel CreateQuizSolverVM()
        //{
        //    //return new QuizSolverViewModel(V, _navigationService);
        //}

    }

}
