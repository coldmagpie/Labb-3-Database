using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lab3_database.DataModels;
using Lab3_database.Managers;

namespace Lab3_database.ViewModels
{
    public class HomeViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        public IRelayCommand NavigateCreateCommand { get; }
        public IRelayCommand NavigateCreateQuestionCommand { get; }
        public IRelayCommand NavigatePlayQuizCommand { get; }
        public IRelayCommand NavigateCreateCategoryCommand { get; }
        public IRelayCommand NavigateEditQuestionCommand { get; }

        public HomeViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            NavigateCreateCommand =
                new RelayCommand(() => _navigationManager.CurrentViewModel = new CreateQuizViewModel(_navigationManager));
            NavigateCreateQuestionCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new CreateQuestionViewModel(_navigationManager));
            NavigateCreateCategoryCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new CreateCategoryViewModel(_navigationManager));
            NavigateEditQuestionCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new EditQuestionViewModel(_navigationManager));
            NavigatePlayQuizCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new ChooseViewModel(_navigationManager));
        } 
    }
}
