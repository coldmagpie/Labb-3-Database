using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lab3_database.DataModels;
using Lab3_database.Managers;

namespace Lab3_database.ViewModels
{
    public class PlayViewModel: ObservableObject
    {
        private NavigationManager _navigationManager;
        private QuizManager _quizManager;
        private Question _question;
        List<Question> temp = new List<Question>();

        public PlayViewModel(NavigationManager navigationManager, Quiz quiz)
        {
            _navigationManager = navigationManager;
            _selectedQuiz = quiz;
            _quizManager = new QuizManager();

            _questionsInQuiz = new ObservableCollection<Question>(_quizManager.GetQuestionsFromQuiz(_selectedQuiz));
            _quizLength = _questionsInQuiz.Count;
            _question = GetRandomQuestion();

            UpdateCorrectCommand = new RelayCommand(() => { ValidateAnswer(); });
            NavigateGoBackCommand =
                new RelayCommand(() => _navigationManager.CurrentViewModel = new HomeViewModel(_navigationManager));
            NextQuestionCommand = new RelayCommand(() =>
            {
                if (QuestionCounter < QuizLength)
                {
                    QuestionCounter++;
                    _question = GetRandomQuestion();
                    OnPropertyChanged(nameof(Statement));
                    OnPropertyChanged(nameof(Answers));
                    CorrectAnswer = string.Empty;
                }
                else
                {
                    _navigationManager.CurrentViewModel = new ResultViewModel(_navigationManager, _score);
                }
            });
        }

        private Question GetRandomQuestion()
        {
            Random random = new Random();
            int index = random.Next(0, _questionsInQuiz.Count());
            Question question = _questionsInQuiz.ElementAt(index);
            _questionsInQuiz = new ObservableCollection<Question>(_questionsInQuiz.Where(q => !q.Statement.Equals(question.Statement)));
            return question;
        }
        public IRelayCommand NavigateGoBackCommand { get; }
        public IRelayCommand UpdateCorrectCommand { get; }
        public IRelayCommand NextQuestionCommand { get; }

        private ObservableCollection<Question> _questionsInQuiz;
        public ObservableCollection<Question> QuestionsInQuiz
        {
            get { return _questionsInQuiz; }
            set { SetProperty(ref _questionsInQuiz, value); }
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

        public int _questionCounter = 1;
        public int QuestionCounter
        {
            get { return _questionCounter; }
            set { SetProperty(ref _questionCounter, value); }
        }
        private int _quizLength;
        public int QuizLength { get => _quizLength; }

        private string _statement;
        public string Statement
        {
            get { return _question.Statement; }
        }
        private string[] _answers;

        public string[] Answers
        {
            get { return _question.Answers; }
        }

        private string _correctAnswer = string.Empty;

        private int _score;

        public int Score
        {
            get { return _score; }
            set
            {
                SetProperty(ref _score, value);
            }
        }

        public string CorrectAnswer
        {
            get
            {
                return _correctAnswer;
            }
            set
            {
                if (value.Equals(""))
                {
                    SetProperty(ref _correctAnswer, value);
                }
                else
                {
                    SetProperty(ref _correctAnswer, (int.Parse(value) + 1).ToString());
                }
            }
        }

        private bool _selectedAnswerOne;
        public bool SelectedAnswerOne
        {
            get
            {
                return _selectedAnswerOne;
            }
            set
            {
                SetProperty(ref _selectedAnswerOne, value);
            }
        }

        private bool _selectedAnswerTwo;

        public bool SelectedAnswerTwo
        {
            get
            {
                return _selectedAnswerTwo;
            }
            set
            {
                SetProperty(ref _selectedAnswerTwo, value);
            }
        }

        private bool _selectedAnswerThree;

        public bool SelectedAnswerThree
        {
            get
            {
                return _selectedAnswerThree;
            }
            set
            {
                SetProperty(ref _selectedAnswerThree, value);
            }
        }

        private bool _selectedAnswerFour;

        public bool SelectedAnswerFour
        {
            get
            {
                return _selectedAnswerFour;
            }
            set
            {
                SetProperty(ref _selectedAnswerFour, value);
            }
        }

        private void ValidateAnswer()
        {
            CorrectAnswer = _question.CorrectAnswer.ToString();
            switch (_question.CorrectAnswer)
            {
                case 0:
                    if (_selectedAnswerOne)
                    {
                        Score++;
                    }
                    break;
                case 1:
                    if (_selectedAnswerTwo)
                    {
                        Score++;
                    }
                    break;
                case 2:
                    if (_selectedAnswerThree)
                    {
                        Score++;
                    }
                    break;
                case 3:
                    if (_selectedAnswerFour)
                    {
                        Score++;
                    }
                    break;
            }
        }
    }
}
