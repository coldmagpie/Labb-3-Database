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
    public class ModifyQuizViewModel:ObservableObject
    {
        private NavigationManager _navigationManager;
        private QuizManager _quizManager;
        private QuestionManager _questionManager;
        private CategoryManager _categoryManager;
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand NavigateAddCommand { get; }
        public IRelayCommand NavigateRemoveCommand { get; }

        private string _quizName;

        public string QuizName
        {
            get { return _quizName; }
            set { SetProperty(ref _quizName, value); }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                SetProperty(ref _categories, value);
            }
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                SetProperty(ref _selectedCategory, value);
                Questions = new ObservableCollection<Question>(_questionManager.QuestionsByCategory(value));
            }
        }

        private ObservableCollection<Quiz> _quizzes;
        public ObservableCollection<Quiz> Quizzes
        {
            get { return _quizzes; }
            set
            {
                SetProperty(ref _quizzes, value);
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
            }
        }

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set
            {
                SetProperty(ref _questions, value);
                OnPropertyChanged(nameof(Questions));
            }
        }

        private Question _selectedQuestion;
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                SetProperty(ref _selectedQuestion, value);
                NavigateAddCommand.NotifyCanExecuteChanged();
            }
        }

        private ObservableCollection<Question> _questionsInQuiz;
        public ObservableCollection<Question> QuestionsInQuiz
        {
            get { return _questionsInQuiz; }
            set
            {
                _questionsInQuiz = value;
                OnPropertyChanged(nameof(QuestionsInQuiz));
            }
        }

        private Question _selectedQuesionInQuiz;
        public Question SelectedQuestionInQuiz
        {
            get { return _selectedQuesionInQuiz; }
            set
            {
                SetProperty(ref _selectedQuesionInQuiz, value);
               NavigateRemoveCommand.NotifyCanExecuteChanged();
            }
        }

        public ModifyQuizViewModel(NavigationManager navigationManager, Quiz quiz)
        {
            _navigationManager = navigationManager;
            _selectedQuiz = quiz;
            _quizName = _selectedQuiz.Title;

            _quizManager = new QuizManager();
            _questionManager = new QuestionManager();
            _categoryManager = new CategoryManager();

            _categories = new ObservableCollection<Category>(_categoryManager.GetAllCategories());
            _quizzes = new ObservableCollection<Quiz>(_quizManager.GetAllQuizzes());
            _questionsInQuiz = new ObservableCollection<Question>(_quizManager.GetQuestionsFromQuiz(_selectedQuiz));
            if (_selectedCategory is null)
            {
                _questions = new ObservableCollection<Question>(_questionManager.GetAllQuestions());
            }

            NavigateAddCommand = new RelayCommand(AddQuestionToQuiz);
            NavigateRemoveCommand = new RelayCommand(RemoveQuestionFromQuiz);
            NavigateGoBackCommand = new RelayCommand(() =>
                _navigationManager.CurrentViewModel = new CreateQuizViewModel(_navigationManager));
        }

        private void UpdateQuestionsInQuiz()
        {
            _questionsInQuiz.Clear();
            foreach (var question in _quizManager.GetQuestionsFromQuiz(_selectedQuiz))
            {
                _questionsInQuiz.Add(question);
            }
        }
        private void AddQuestionToQuiz()
        {
            if (_selectedQuestion is null )
            {
                return;
            }
            _quizManager.AddQuestionToQuiz(_selectedQuiz.Id, _selectedQuestion);
            UpdateQuestionsInQuiz();
        }

        private void RemoveQuestionFromQuiz()
        {
            if (_selectedQuesionInQuiz is null)
            {
                return;
            }
            _quizManager.RemoveQuestionFromQuiz(_selectedQuiz.Id, _selectedQuesionInQuiz.Id);
            UpdateQuestionsInQuiz();
        }
    }
}
