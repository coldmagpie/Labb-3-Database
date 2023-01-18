using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lab3_database.DataModels
{
    public class Question
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement]
        public string Statement { get; set; }
        [BsonElement]
        public string[] Answers { get; set; }
        [BsonElement]
        public int CorrectAnswer { get; set; }

        [BsonElement]
        public List<Category> Categories { get; set; }
        public Question(string statement, string[] answers, int correctAnswer, List<Category> categories)
        {
            Statement = statement;
            Answers = answers;
            CorrectAnswer = correctAnswer;
            Categories = categories;
        }
    }
}
