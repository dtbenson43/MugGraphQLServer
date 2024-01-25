using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Mug.Services.CosmosDb.Models;
using Mug.Services.CosmosDb.Models.ChooseGame;

namespace Mug.Services.CosmosDb
{
    public class CosmosDbService
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public CosmosDbService(string connectionString, string databaseName)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public IMongoDatabase Database { get { return _database; } }

        public IMongoCollection<T> StoreCollection<T>() where T : class => _database.GetCollection<T>("store");

        public IMongoCollection<T> ConversationsCollection<T>() where T : class => _database.GetCollection<T>("conversations");

        public IMongoCollection<TestDocument> TestDocument() => StoreCollection<TestDocument>();

        public IMongoCollection<ConversationMessage> Conversations { get { return ConversationsCollection<ConversationMessage>(); } }

        public IMongoCollection<ChooseGame> ChooseGames => _database.GetCollection<ChooseGame>("chooseGames");

        public async Task AddChooseGameAsync(ChooseGame game) => await ChooseGames.InsertOneAsync(game);
    }
}
