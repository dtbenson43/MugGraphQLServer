using MongoDB.Bson.Serialization.Attributes;

namespace Mug.Services.CosmosDb.Models
{
    [BsonIgnoreExtraElements]
    public class ConversationMessage
    {
        public string ConversationId { get; set; } = null!;
        public string MessageId { get; set;} = null!;
        public string? ParentId { get; set; }
        public string UserId { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string From { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
