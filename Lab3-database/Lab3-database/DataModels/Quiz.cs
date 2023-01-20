using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Lab3_database.DataModels
{
    public class Quiz
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement]
        public string Title { get; set; }
        [BsonElement]
        public List<Question> Questions { get; set; } = new();

    }
}
