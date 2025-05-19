using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Quiz_Solver_App.ViewModel.Base;

namespace Quiz_Solver_App.Services
{
    public class NavigationService
    {
        private Action<ViewModelBase> _navigate;
        public void SetNavigator(Action<ViewModelBase> navigator)
        {
            _navigate = navigator;
        }

        public void NavigateTo(ViewModelBase viewModel)
        {
            _navigate?.Invoke(viewModel);
        }
    }
}
