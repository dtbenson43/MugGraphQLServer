using MongoDB.Bson.Serialization.Attributes;

namespace Mug.Services.CosmosDb.Models.Chat
{
    [BsonIgnoreExtraElements]
    public class ChatMessage
    {
        public string Id { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Message { get; set; } = null!;

        public string Channel {  get; set; } = null!;

        public DateTime DateTime { get; set; }
    }
}
