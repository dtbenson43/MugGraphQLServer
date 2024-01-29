using Mug.Services.CosmosDb.Models.ChooseGame;
using Mug.Services.CosmosDb;
using Mug.Services.OpenAI;
using Mug.Services.OpenAI.Models;
using HotChocolate.Authorization;
using Mug.Utilities;

namespace Mug.Mutation
{
    public partial class Mutation
    {
        [Authorize]
        public async Task<CreateNewGamePayload> CreateNewGame(string userId, [Service] CosmosDbService cosmos, [Service] OpenAIService openAI)
        {
            ChooseGameBranchData branchData = await openAI.CreateChooseGameBranchAsync();
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
                Text = branchData.Text,
                CreatedAt = DateTime.Now,
                FirstOption = new ChoiceOption {
                    Text = branchData.FirstOption,
                    NextBranchId = Guid.NewGuid().ToString(),
                },
                SecondOption = new ChoiceOption
                {
                    Text = branchData.SecondOption,
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

        [Authorize]
        public async Task<AddUserSelectionPayload> AddUserSelection(string gameId, string choiceId, [Service] CosmosDbService cosmos, [Service] OpenAIService openAI)
        {
            var game = await cosmos.GetChooseGameByIdAsync(gameId);
            var currentBranch = game.CurrentBranch;
            var userChoide = ChooseGameUtilities.GetUserChoiceOptionById(currentBranch, choiceId);
            
            currentBranch.UserChoice = userChoide;

            if (game.InitialBranch.Id == currentBranch.Id) game.InitialBranch.UserChoice = userChoide;
            else
            {
                var cur = ChooseGameUtilities.GetBranchById(game, currentBranch.Id);
                if (cur == null) throw new Exception("Branch data malformed");
                cur.UserChoice = userChoide;
            }

            var branchData = await openAI.CreateChooseGameBranchAsync(game);
            var newBranch = new ChooseGameBranch()
            {
                Id = choiceId, // Generate a new GUID for the branch ID
                Text = branchData.Text,
                CreatedAt = DateTime.Now,
                FirstOption = new ChoiceOption
                {
                    Text = branchData.FirstOption,
                    NextBranchId = Guid.NewGuid().ToString(),
                },
                SecondOption = new ChoiceOption
                {
                    Text = branchData.SecondOption,
                    NextBranchId = Guid.NewGuid().ToString(),
                },
            };

            game.CurrentBranch = newBranch;
            game.PreviousBranch = currentBranch;
            game.Branches.Add(newBranch);

            await cosmos.ReplaceChooseGameAsync(game);

            return new AddUserSelectionPayload
            {
                UpdatedGame = game,
            };
        }
    }

    public class AddUserSelectionPayload
    {
        public ChooseGame UpdatedGame { get; set; } = null!;

    }

    public class CreateNewGamePayload
    {
        public ChooseGame NewGame { get; set; } = null!;
    }
}
