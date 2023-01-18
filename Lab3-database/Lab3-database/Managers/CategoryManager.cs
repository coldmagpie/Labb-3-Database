using Lab3_database.DataModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_database.Managers
{
    public class CategoryManager
    {
        private readonly IMongoCollection<Category> _categories;
        public CategoryManager()
        {
            var Hostname = "localhost";
            var Port = "27017";
            var DatabaseName = "QuizGame";
            var ConnectionString = $"mongodb://{Hostname}:{Port}";
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);
            _categories = database.GetCollection<Category>("categories",
                new MongoCollectionSettings() { AssignIdOnInsert = true });
        }
        public IEnumerable<Category> GetAllCategories()
        {
            return _categories.Find(_ => true).ToEnumerable();
        }
        public void CreateCategory(string name)
        {
            var newCategory = new Category()
            {
                Id = ObjectId.GenerateNewId(),
                Name = name,
            };
            _categories.InsertOne(newCategory);
        }
        public void UpdateCategory(object id, string name)
        {
            var filter = Builders<Category>.Filter.Eq("Id", id);
            var update = Builders<Category>.Update.Set("Name", name);

            _categories.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<Category, Category>
            {
                IsUpsert = true
            });
        }
        public void DeleteCategory(object id)
        {
            var filter = Builders<Category>.Filter.Eq("Id", id);
            _categories.FindOneAndDelete(filter);
        }
    }
}
