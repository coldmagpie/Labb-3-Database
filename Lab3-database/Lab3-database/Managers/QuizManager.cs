using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3_database.DataModels;
using MongoDB.Bson;

namespace Lab3_database.Managers
{
    public class QuizManager
    {
        private readonly IMongoCollection<Quiz> _quizzes;
        
        public QuizManager()
        {
            var Hostname = "localhost";
            var Port = "27017";
            var DatabaseName = "QuizGame";
            var ConnectionString = $"mongodb://{Hostname}:{Port}";
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);
            _quizzes = database.GetCollection<Quiz>("quizzes",
                new MongoCollectionSettings() { AssignIdOnInsert = true });
        }

        public IEnumerable<Quiz> GetAllQuizzes()
        {
            return _quizzes.Find(_ => true).ToEnumerable();
        }
        public Quiz CreateQuiz(string title)
        {
            var quiz = new Quiz
            {
                Title = title
            };
            _quizzes.InsertOne(quiz);
            return quiz;
        }

        public void UpdateQuiz(object id, string title)
        {
            var filter = Builders<Quiz>.Filter.Eq("Id", id);
            var update = Builders<Quiz>.Update.Set("Title", title);

            _quizzes.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<Quiz, Quiz>
            {
                IsUpsert = true
            });
        }
        public void DeleteQuiz(object id)
        {
            var filter = Builders<Quiz>.Filter.Eq("Id", id);
            _quizzes.FindOneAndDelete(filter);
        }

        public IEnumerable<Question> GetQuestionsFromQuiz(Quiz quiz)
        {
            
            return _quizzes.Find(q=> quiz.Id.Equals(q.Id)).FirstOrDefault().Questions.ToList();
        }
        public void AddQuestionToQuiz(object id, Question question)
        {
            var quiz = _quizzes.Find(q => q.Id == (ObjectId)id).FirstOrDefault();
            if (quiz == null)
                return;
            if (quiz.Questions.Any(q=> q.Id.ToString() == question.Id.ToString()))
            {
                return;
            }
            var filter = Builders<Quiz>.Filter.Eq("Id", id);
            var update = Builders<Quiz>.Update.Push(q => q.Questions, question);
            _quizzes.UpdateOne(filter, update);
        }
        public void RemoveQuestionFromQuiz(object quizId, ObjectId questionId)
        {
            var filter = Builders<Quiz>.Filter.Eq(q => q.Id, quizId);
            var update = Builders<Quiz>.Update.PullFilter(q => q.Questions, question => question.Id == questionId);
            _quizzes.UpdateOne(filter, update);
        }
        private Quiz _currentQuiz;
        public Quiz CurrentQuiz
        {
            get { return _currentQuiz; }
            set
            {
                _currentQuiz = value;
                CurrentQuizChanged?.Invoke();
            }
        }

        public event Action CurrentQuizChanged;
    }

}
