using MongoDB.Bson.Serialization.Attributes;

namespace Mug.Services.CosmosDb.Models
{
    [BsonIgnoreExtraElements]
    public class TestDocument
    {
        public string Uuid { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
}
