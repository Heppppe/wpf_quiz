using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Quiz_Solver_App.Services;
using Quiz_Solver_App.ViewModel.Base;
using Microsoft.Win32;
using System.Windows.Input;

namespace Quiz_Solver_App.ViewModel
{
    public class MainMenuViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly ViewModelFactory _viewModelFactory;

        public ICommand SelectJsonFileCommand { get; }
        private string _selectedJsonFilePath;
        public string SelectedJsonFilePath
        {
            get => _selectedJsonFilePath;
            set
            {
                _selectedJsonFilePath = value;
                OnPropertyChanged(nameof(SelectedJsonFilePath));
            }
        }

        public MainMenuViewModel(NavigationService navigationService, ViewModelFactory viewModelFactory)
        {
            _navigationService = navigationService;
            _viewModelFactory = viewModelFactory;

            SelectJsonFileCommand = new RelayCommand(_ => SelectJsonFile(), _ => true);
        }

        private void SelectJsonFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki JSON (*.json)|*.json",
                Title = "Wybierz plik quizu"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedJsonFilePath = openFileDialog.FileName;
                // Tutaj możesz dodać kod do wczytania i przetworzenia pliku
            }
        }
    }

}

