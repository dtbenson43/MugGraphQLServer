using MongoDB.Bson.Serialization.Attributes;

namespace Mug.Services.CosmosDb.Models.ChooseGame
{
    [BsonIgnoreExtraElements]
    public class ChooseGameBranch
    {
        public string Id { get; set; } = null!;

        public string Text { get; set; } = null!; 

        public ChoiceOption FirstOption { get; set; } = null!;

        public ChoiceOption SecondOption { get; set; } = null!;

        public UserChoiceOption? UserChoice { get; set; }
    }

    public class ChoiceOption
    {
        public string Text { get; set; } = null!;
        public string NextBranchId { get; set; } = null!;
    }

    public enum UserChoiceOption
    {
        FirstOption = 1,
        SecondOption = 2
    }
}
