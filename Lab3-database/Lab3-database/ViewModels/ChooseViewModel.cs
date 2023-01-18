using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lab3_database.DataModels;
using Lab3_database.Managers;

namespace Lab3_database.ViewModels
{
    public class ChooseViewModel :ObservableObject
    {
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateConfirmCommand { get; }

        private NavigationManager _navigationManager;
        private QuizManager _quizManager;
    

        private ObservableCollection<Quiz> _quizzes;
        public ObservableCollection<Quiz> Quizzes
        {
            get { return _quizzes; }
            set { _quizzes = value; }
        }

        private Quiz _selectedQuiz;
        public Quiz SelectedQuiz
        {
            get { return _selectedQuiz; }
            set
            {
                SetProperty(ref _selectedQuiz, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        public ChooseViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _quizManager = new QuizManager();

            _quizzes = new ObservableCollection<Quiz>(_quizManager.GetAllQuizzes());

            NavigateGoBackCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new HomeViewModel(_navigationManager));

            NavigateConfirmCommand = new RelayCommand(() =>
                    _navigationManager.CurrentViewModel = 
                        new PlayViewModel(_navigationManager, _selectedQuiz), () =>_selectedQuiz!= null);
            
        }
    }
}
