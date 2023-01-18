using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lab3_database.DataModels;
using Lab3_database.Managers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lab3_database.ViewModels
{
    public class CreateQuizViewModel:ObservableObject
    {
        NavigationManager _navigationManager;
        private QuizManager _quizManager;
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateConfirmCommand { get; }
        public IRelayCommand NavigateUpdateCommand { get; }
        public IRelayCommand NavigateDeleteCommand { get; }
        public IRelayCommand NavigateToModifyCommand { get; }



        private string _newQuiz;

        public string NewQuiz
        {
            get { return _newQuiz; }
            set 
            {
                SetProperty(ref _newQuiz, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }
        private string _newName;
        public string NewName
        {
            get { return _newName; }
            set
            {
                SetProperty(ref _newName, value);
                NavigateUpdateCommand.NotifyCanExecuteChanged();
                NavigateUpdateCommand.NotifyCanExecuteChanged();
            }
        }

        private ObservableCollection<Quiz> _quizzes;
        public ObservableCollection<Quiz> Quizzes
        {
            get { return _quizzes; }
            set
            {
                _quizzes = value;
                OnPropertyChanged(nameof(Quizzes));
            }
        }

        private Quiz _selectedQuiz;

        public Quiz SelectedQuiz
        {
            get { return _selectedQuiz; }
            set
            {
                SetProperty(ref _selectedQuiz, value);
                NavigateDeleteCommand.NotifyCanExecuteChanged();
                NavigateUpdateCommand.NotifyCanExecuteChanged();
                NavigateToModifyCommand.NotifyCanExecuteChanged();

                if(_selectedQuiz != null)
                {
                    NewName = _selectedQuiz.Title;
                }
            }
        }

        public CreateQuizViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _quizManager = new QuizManager();
            _quizzes = new ObservableCollection<Quiz>(_quizManager.GetAllQuizzes());
            NavigateGoBackCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = 
                new HomeViewModel(_navigationManager));
            NavigateConfirmCommand = new RelayCommand(CreateQuiz, ()=> !string.IsNullOrEmpty(NewQuiz));
            NavigateUpdateCommand = new RelayCommand(UpdateQuiz, () => _selectedQuiz != null);
            NavigateDeleteCommand = new RelayCommand(DeleteQuiz, () => _selectedQuiz != null);
            NavigateToModifyCommand = new RelayCommand(()=> _navigationManager.CurrentViewModel =
                new ModifyQuizViewModel(_navigationManager, _selectedQuiz), ()=>_selectedQuiz!= null);
        }

        private void CreateQuiz()
        {
            _quizManager.CreateQuiz(NewQuiz);
            NewQuiz = string.Empty;
            UpdateQuizzes();
        }
        private void UpdateQuiz()
        {
            _quizManager.UpdateQuiz(_selectedQuiz.Id, NewName);
            UpdateQuizzes();
            NewName = string.Empty;
        }
        private void DeleteQuiz()
        {
            _quizManager.DeleteQuiz(_selectedQuiz.Id);
            UpdateQuizzes();
            NewName = string.Empty;
        }
        private void UpdateQuizzes()
        {
            Quizzes.Clear();
            foreach (var quiz in _quizManager.GetAllQuizzes())
            {
                Quizzes.Add(quiz);
            }
        }
    }
}
