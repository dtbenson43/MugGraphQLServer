using Mug.Services.CosmosDb.Models.ChooseGame;
using Mug.Services.CosmosDb;
using Mug.Services.OpenAI;
using Mug.Services.OpenAI.Models;
using HotChocolate.Authorization;

namespace Mug.Mutation
{
    public partial class Mutation
    {
        [Authorize]
        public async Task<CreateNewGamePayload> CreateNewGame(string userId, [Service] CosmosDbService cosmos, [Service] OpenAIService openAI)
        {
            CreateChooseGameBranchResponse branch = await openAI.CreateChooseGameBranch();
            // Create a new ChooseGame object
            var newGame = new ChooseGame
            {
                Id = Guid.NewGuid().ToString(), // Generate a new GUID for the game ID
                UserId = userId,
                Title = "Alone In Space",
                CreatedAt = DateTime.UtcNow,
                LastAccessed = DateTime.UtcNow,
                Branches = []
            };

            // Create an initial branch
            var initialBranch = new ChooseGameBranch
            {
                Id = Guid.NewGuid().ToString(), // Generate a new GUID for the branch ID
                Text = branch.Text,
                CreatedAt = DateTime.Now,
                FirstOption = new ChoiceOption {
                    Text = branch.FirstOption,
                    NextBranchId = Guid.NewGuid().ToString(),
                },
                SecondOption = new ChoiceOption
                {
                    Text = branch.SecondOption,
                    NextBranchId = Guid.NewGuid().ToString(),
                },
            };

            // Add the initial branch to the new game
            newGame.InitialBranch = initialBranch;
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
