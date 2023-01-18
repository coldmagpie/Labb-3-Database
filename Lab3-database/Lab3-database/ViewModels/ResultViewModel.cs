using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lab3_database.Managers;

namespace Lab3_database.ViewModels
{
    public class ResultViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        public IRelayCommand NavigateExitCommand { get; }
        public ResultViewModel(NavigationManager navigationManager, int score)
        {
            _navigationManager = navigationManager;
            _score = score;
            NavigateExitCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new HomeViewModel(_navigationManager));
        }

        private int _score;
        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

    }
}
