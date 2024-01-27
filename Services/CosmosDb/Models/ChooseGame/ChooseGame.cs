using MongoDB.Bson.Serialization.Attributes;

namespace Mug.Services.CosmosDb.Models.ChooseGame
{
    [BsonIgnoreExtraElements]
    public class ChooseGame
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime LastAccessed { get; set; }

        public ChooseGameBranch CurrentBranch { get; set; } = null!;

        public ChooseGameBranch InitialBranch { get; set; } = null!;
        
        public ChooseGameBranch? PreviousBranch { get; set; }

        public List<ChooseGameBranch> Branches { get; set; } = null!;

    }
}
