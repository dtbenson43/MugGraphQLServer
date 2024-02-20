using MongoDB.Bson.Serialization.Attributes;

namespace Mug.Services.CosmosDb.Models.Infinichemy
{
    [BsonIgnoreExtraElements]
    public class InfElement
    {
        public string ElementId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Emoji { get; set; } = null!;

    }
}
