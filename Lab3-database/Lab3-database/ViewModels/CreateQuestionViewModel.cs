using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lab3_database.DataModels;
using Lab3_database.Managers;

namespace Lab3_database.ViewModels
{
    public class CreateQuestionViewModel : ObservableObject
    {
        private NavigationManager _navigationManager;
        private QuestionManager _questionManager;
        private CategoryManager _categoryManager;
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateConfirmCommand { get; }

        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        private string _statement;

        public string Statement
        {
            get { return _statement; }
            set
            {
                SetProperty(ref _statement, value); 
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private string _answer1;

        public string Answer1
        {
            get { return _answer1; }
            set
            {
                SetProperty(ref _answer1, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private string _answer2;

        public string Answer2
        {
            get { return _answer2; }
            set
            {
                SetProperty(ref _answer2, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private string _answer3;

        public string Answer3
        {
            get { return _answer3; }
            set
            {
                SetProperty(ref _answer3, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private string _answer4;

        public string Answer4
        {
            get { return _answer4; }
            set
            {
                SetProperty(ref _answer4, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        private int _correctAnswer;

        public int CorrectAnswer
        {
            get { return _correctAnswer; }
            set
            {
                SetProperty(ref _correctAnswer, value);
                NavigateConfirmCommand.NotifyCanExecuteChanged();
            }
        }

        public CreateQuestionViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _questionManager = new QuestionManager();
            _categoryManager = new CategoryManager();
            _categories = new ObservableCollection<Category>(_categoryManager.GetAllCategories());

            NavigateConfirmCommand = new RelayCommand(CreateQuestion, CanExecute);
            NavigateGoBackCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new HomeViewModel(_navigationManager));
        }
        private void CreateQuestion()
        {
            string[] Answers = { Answer1, Answer2, Answer3, Answer4 };
            _questionManager.CreateQuestion(
                new Question(Statement, Answers, CorrectAnswer, Categories.Where(category => category.IsSelected).ToList()));
            Statement = string.Empty;
            Answer1 = string.Empty;
            Answer2 = string.Empty;
            Answer3 = string.Empty;
            Answer4 = string.Empty;
        }

        private bool CanExecute()
        {
            return !string.IsNullOrEmpty(Statement) &&
                   !string.IsNullOrEmpty(Answer1) &&
                   !string.IsNullOrEmpty(Answer2) &&
                   !string.IsNullOrEmpty(Answer3) &&
                   !string.IsNullOrEmpty(Answer4) &&
                   CorrectAnswer != null;
        }
    }
}
