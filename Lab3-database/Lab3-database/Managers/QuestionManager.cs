using Lab3_database.DataModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_database.Managers
{
    public class QuestionManager
    {
        private readonly IMongoCollection<Question> _questions;
        public QuestionManager()
        {
            var Hostname = "localhost";
            var Port = "27017";
            var DatabaseName = "QuizGame";
            var ConnectionString = $"mongodb://{Hostname}:{Port}";
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);
            _questions = database.GetCollection<Question>("questions",
                new MongoCollectionSettings() { AssignIdOnInsert = true });
        }
        public IEnumerable<Question> GetAllQuestions()
        {
            return _questions.Find(_ => true).ToEnumerable();
        }
        public void CreateQuestion(Question question)
        {
            _questions.InsertOne(question);
        }
        public void UpdateQuestion(object id, Question question)
        {
            var filter = Builders<Question>.Filter.Eq("Id", id);
            var update = Builders<Question>.Update
                .Set("Statement", question.Statement)
                .Set("Answers", question.Answers)
                .Set("CorrectAnswer", question.CorrectAnswer)
                .Set("Categories", question.Categories);

            _questions.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<Question, Question>
            {
                IsUpsert = true
            });
        }
        public void DeleteQuestion(object id)
        {
            var filter = Builders<Question>.Filter.Eq("Id", id);
            _questions.FindOneAndDelete(filter);
        }
        public IEnumerable<Question> QuestionsByCategory(Category category)
        {
            return _questions.Aggregate().ToList()
                .FindAll(question => question.Categories.Any(c => c.Id == category.Id));
        }
    }
}
