using MongoDB.Bson.Serialization.Attributes;

namespace Mug.Services.CosmosDb.Models.Infinichemy
{
    [BsonIgnoreExtraElements]
    public class InfCombination
    {
        public string CombinationId { get; set; } = null!;

        public string ElementOne { get; set; } = null!;

        public string ElementTwo { get; set; } = null!;

        public string ResultElement { get; set; } = null!;

        public string ResultElementId { get; set; } = null!;

    }
}
