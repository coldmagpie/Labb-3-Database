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
    public class EditQuestionViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        private QuestionManager _questionManager;
        private CategoryManager _categoryManager;
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateUpdateCommand { get; }
        public IRelayCommand NavigateDeleteCommand { get; }

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value; 
                OnPropertyChanged(nameof(Questions));
            }
        }
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                SetProperty(ref _selectedQuestion, value);
                NavigateDeleteCommand.NotifyCanExecuteChanged();
                NavigateUpdateCommand.NotifyCanExecuteChanged();

                if (_selectedQuestion != null)
                {
                    NewStatement = _selectedQuestion.Statement;
                    Answer1 = _selectedQuestion.Answers[0];
                    Answer2 = _selectedQuestion.Answers[1];
                    Answer3 = _selectedQuestion.Answers[2];
                    Answer4 = _selectedQuestion.Answers[3];
                    NewCorrectAnswer = _selectedQuestion.CorrectAnswer;
                }
            }
        }

        private string _newStatement;
        public string NewStatement
        {
            get { return _newStatement; }
            set
            {
                SetProperty(ref _newStatement, value);
                NavigateUpdateCommand.NotifyCanExecuteChanged();
                NavigateDeleteCommand.NotifyCanExecuteChanged();
            }
        }

        private string _answer1;

        public string Answer1
        {
            get { return _answer1; }
            set
            {
                SetProperty(ref _answer1, value);
                NavigateUpdateCommand.NotifyCanExecuteChanged();
                NavigateDeleteCommand.NotifyCanExecuteChanged();
            }
        }

        private string _answer2;

        public string Answer2
        {
            get { return _answer2; }
            set
            {
                SetProperty(ref _answer2, value); 
                NavigateUpdateCommand.NotifyCanExecuteChanged();
                NavigateDeleteCommand.NotifyCanExecuteChanged();
            }
        }

        private string _answer3;

        public string Answer3
        {
            get { return _answer3; }
            set
            {
                SetProperty(ref _answer3, value);
                NavigateUpdateCommand.NotifyCanExecuteChanged();
                NavigateDeleteCommand.NotifyCanExecuteChanged();
            }
        }

        private string _answer4;

        public string Answer4
        {
            get { return _answer4; }
            set
            {
                SetProperty(ref _answer4, value);
                NavigateUpdateCommand.NotifyCanExecuteChanged();
                NavigateDeleteCommand.NotifyCanExecuteChanged();
            }
        }


        private int _newCorrectAnswer;

        public int NewCorrectAnswer
        {
            get { return _newCorrectAnswer; }
            set { SetProperty(ref _newCorrectAnswer, value); }
        }
        public EditQuestionViewModel(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _questionManager = new QuestionManager();
            _categoryManager = new CategoryManager();
            _questions = new ObservableCollection<Question>(_questionManager.GetAllQuestions());
            _categories = new ObservableCollection<Category>(_categoryManager.GetAllCategories());

            NavigateUpdateCommand = new RelayCommand(UpdateQuestion, CanExecute);
            NavigateDeleteCommand = new RelayCommand(DeleteQuestion, CanExecute);
            NavigateGoBackCommand =
                new RelayCommand(()=> _navigationManager.CurrentViewModel = new HomeViewModel(_navigationManager));

        }
        private void UpdateQuestion()
        {
            string[] Answers = { Answer1, Answer2, Answer3, Answer4 };
            _questionManager.UpdateQuestion(_selectedQuestion.Id,
                new Question(NewStatement, Answers, NewCorrectAnswer, Categories.Where(category => category.IsSelected).ToList()));
            UpdateQuestions();
            NewStatement = string.Empty;
            Answer1 = string.Empty;
            Answer2 = string.Empty;
            Answer3 = string.Empty;
            Answer4 = string.Empty;
        }
        private void DeleteQuestion()
        {
            _questionManager.DeleteQuestion(_selectedQuestion.Id);
            UpdateQuestions();
            NewStatement = string.Empty;
            Answer1 = string.Empty;
            Answer2 = string.Empty;
            Answer3 = string.Empty;
            Answer4 = string.Empty;
        }
        private void UpdateQuestions()
        {
            Questions.Clear();
            foreach (var question in _questionManager.GetAllQuestions())
            {
                Questions.Add(question);
            }
        }
        private bool CanExecute()
        {
            return !string.IsNullOrEmpty(NewStatement) &&
                   !string.IsNullOrEmpty(Answer1) &&
                   !string.IsNullOrEmpty(Answer2) &&
                   !string.IsNullOrEmpty(Answer3) &&
                   !string.IsNullOrEmpty(Answer4);
        }
    }
}
