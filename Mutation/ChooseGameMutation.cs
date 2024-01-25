using Mug.Services.CosmosDb.Models.ChooseGame;
using Mug.Services.CosmosDb;

namespace Mug.Mutation
{
    public partial class Mutation
    {
        public async Task<CreateNewGamePayload> CreateNewGame(string userId, [Service] CosmosDbService cosmos)
        {
            // Create a new ChooseGame object
            var newGame = new ChooseGame
            {
                Id = Guid.NewGuid().ToString(), // Generate a new GUID for the game ID
                UserId = userId,
                Branches = new List<ChooseGameBranch>()
            };

            // Create an initial branch
            var initialBranch = new ChooseGameBranch
            {
                Id = Guid.NewGuid().ToString(), // Generate a new GUID for the branch ID
                Text = "Initial branch text", // Placeholder text
                FirstOption = new ChoiceOption {
                    Text = "First option text", // Placeholder text
                    NextBranchId = Guid.NewGuid().ToString(),   
                },
                SecondOption = new ChoiceOption
                {
                    Text = "Second option text", // Placeholder text
                    NextBranchId = Guid.NewGuid().ToString(),
                },
            };

            // Add the initial branch to the new game
            newGame.CurrentBranch = initialBranch;
            newGame.Branches.Add(initialBranch);

            // Save the new game to the database
            await cosmos.AddChooseGameAsync(newGame);

            return new CreateNewGamePayload
            {
                NewGame = newGame,
            };
        }
    }

    public class CreateNewGamePayload
    {
        public ChooseGame NewGame { get; set; } = null!;
    }
}
