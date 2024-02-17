using MongoDB.Bson.Serialization.Attributes;

namespace Mug.Services.CosmosDb.Models.Infinichemy
{
    [BsonIgnoreExtraElements]
    public class CombinationResult
    {
        public string Hash { get; set; } = null!;

        public string ElementOne { get; set; } = null!;

        public string ElementTwo { get; set; } = null!;

        public string Result { get; set; } = null!;

    }
}
