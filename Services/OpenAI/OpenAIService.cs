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
                Model = Model.GPT4_Turbo,
                Temperature = 0.0,
                MaxTokens = 500,
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
                "scene and the perfect time for slower scene. You always output JSON in the " +
                "form of { \"Text\": \"\", \"FirstOption\": \"\", \"SecondOption\":\"\" }"
            )
            };

            ChooseGameBranch? currentBranch = null;
            if (game != null) currentBranch = ChooseGameUtilities.GetNextBranch(game, null);
            if (currentBranch == null || game == null)
            {
                messages.Add(new(ChatMessageRole.User,
                    "The user is starting a new game. The game is titled \"Stranded In Space\". " +
                    "The narrative is about a small crew of a spacecraft on a colony mission. " +
                    "The colonists are in hypersleep and the crew must safely transport them to " +
                    "the destination planet. Unbeknownst to the crew a single elusive alien with " +
                    "unknown abilities has infiltrated the spacecraft. Create the inital branch."
                ));
            } 
            else
            {
                while (ChooseGameUtilities.GetNextBranch(game, currentBranch, out currentBranch))
                {
                    if (currentBranch == null) break;
                    messages.Add(new(
                        ChatMessageRole.User,
                        $"{currentBranch.Text}\nUser Choice: {ChooseGameUtilities.GetUserChoice(currentBranch)?.Text ?? ""}")
                    );
                }
            }
            return [.. messages];
        }
    }
}
