using MongoDB.Bson;
using Mug.Services.CosmosDb.Models.ChooseGame;
using Mug.Services.OpenAI.Models;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using Mug.Utilities;

namespace Mug.Services.OpenAI
{
    public class OpenAIService
    {
        private OpenAIAPI _openai;
        public OpenAIService(string OpenAIKey)
        {
            _openai = new OpenAIAPI(OpenAIKey);
        }

        public OpenAIAPI Client { get { return _openai; } }

        public async Task<CreateChooseGameBranchResponse> CreateChooseGameBranch(ChooseGame? game = null)
        {
            var messages = CreateChooseGameChatMessages(game);
            ChatRequest chatRequest = new ChatRequest()
            {
                Model = "gpt-4-turbo-preview",
                Temperature = 0.1,
                MaxTokens = 4096,
                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                Messages = messages
            };

            var tries = 0;
            CreateChooseGameBranchResponse? results = null;
            while (tries < 3 && results == null)
            {
                try
                {
                    var response = await _openai.Chat.CreateChatCompletionAsync(chatRequest);
                    if (response != null)
                        results = JsonConvert.DeserializeObject<CreateChooseGameBranchResponse>(response.ToString());
                }
                catch (Exception)
                {
                    if (tries == 2) throw;
                }
                finally
                {
                    tries += 1;
                }
            }

            return results ?? new CreateChooseGameBranchResponse();
        }

        private ChatMessage[] CreateChooseGameChatMessages(ChooseGame? game)
        {
            var messages = new List<ChatMessage>
            {
                new(ChatMessageRole.System,
                "You are a helpful writing assistant designed to output JSON.You will be " +
                "creating branches for a choose your own adventure story. You will come " +
                "up with the next 'Text' (the next promprt for the user), 'FirstOption' " +
                "(the first option a user can select) and 'SecondOption' (the second option " +
                "a user can select). You always seem to know the perfect time for a dramatic " +
                "scene and the perfect time for slower scene. Be sure to properly develop the " +
                "story and provide details of the settings plot and characters where appropriate. " +
                "Pleae write at least a paragraph. You always output JSON in the form " +
                "of { \"Text\": \"\", \"FirstOption\": \"\", \"SecondOption\":\"\" }\n"
            ),
                new(ChatMessageRole.System,
                "The user is starting a new game. The game is titled \"Stranded In Space\". " +
                "The narrative is about a small three person crew of a spacecraft on a colony mission. " +
                "The colonists are in hypersleep and the crew must safely transport them to " +
                "the destination planet. Unbeknownst to the crew a single elusive alien " +
                "has infiltrated the spacecraft. The alien is extremely fast, aggressive " +
                "and deadly. Create the inital branch."
            )
            };

            ChooseGameBranch? currentBranch = null;
            while (game != null && ChooseGameUtilities.GetNextBranch(game, currentBranch, out currentBranch))
            {
                if (currentBranch == null) break;
                messages.Add(new(
                    ChatMessageRole.Assistant,
                    $"\n{currentBranch.Text}" +
                    $"\nOption one: {currentBranch.FirstOption.Text}" +
                    $"\nOption two: {currentBranch.SecondOption.Text}")
                );

                var userChoice = ChooseGameUtilities.GetUserChoice(currentBranch);
                var userChoiceOption = currentBranch.UserChoice;

                if (userChoiceOption == null || userChoice == null)
                    throw new Exception("User hasn't made a selection yet!");

                messages.Add(new(
                    ChatMessageRole.User,
                    $"\nUser Choice: {userChoiceOption} - {userChoice.Text}"));
            }
            return [.. messages];
        }
    }
}
