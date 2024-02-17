using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Mug.Services.CosmosDb.Models;
using Mug.Services.CosmosDb.Models.Chat;
using Mug.Services.CosmosDb.Models.ChooseGame;
using Mug.Services.CosmosDb.Models.Infinichemy;

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

        public IMongoCollection<CombinationResult> Infinichemy => _database.GetCollection<CombinationResult>("infinichemy");


        public IMongoCollection<ChatMessage> Chat => _database.GetCollection<ChatMessage>("chat");


        public async Task AddChooseGameAsync(ChooseGame game) => await ChooseGames.InsertOneAsync(game);

        public async Task<ChooseGame> GetChooseGameByIdAsync(string id)
        {
            return await ChooseGames.Find(game => game.Id == id).FirstOrDefaultAsync();
        }

        public async Task ReplaceChooseGameAsync(ChooseGame newGameData)
        {
            await ChooseGames.ReplaceOneAsync(game => game.Id == newGameData.Id, newGameData);
        }

        public async Task AddChatMessageAsync(ChatMessage message) => await Chat.InsertOneAsync(message);

        public async Task<CombinationResult> GetCombinationResultByHashAsync(string hash)
        {
            return await Infinichemy.Find(result => result.Hash == hash).FirstOrDefaultAsync();
        }

        public async Task AddCombinationResultAsync(CombinationResult result)
        {
            // Before inserting, you might want to ensure that there isn't already a document with the same hash.
            // This check acts as an additional safeguard on top of the unique index constraint you should have on the 'hash' field.
            var existingResult = await GetCombinationResultByHashAsync(result.Hash);
            if (existingResult == null)
            {
                await Infinichemy.InsertOneAsync(result);
            }
            else
            {
                throw new InvalidOperationException("A result with the same hash already exists.");
            }
        }
    }
}
