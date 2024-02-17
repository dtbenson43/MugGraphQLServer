using MongoDB.Bson;
using Mug.Services.CosmosDb.Models.ChooseGame;
using Mug.Services.OpenAI.Models;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using Mug.Utilities;
using Microsoft.Extensions.Azure;
using static HotChocolate.ErrorCodes;
using Azure;

namespace Mug.Services.OpenAI
{

    public partial class OpenAIService
    {
        public async Task<string> GetCombination(string one, string two)
        {
            ChatRequest chatRequest = new ChatRequest()
            {
                Model = "gpt-3.5-turbo-1106",
                TopP = 1,
                Temperature = 0,
                MaxTokens = 4096,
                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                Messages = new ChatMessage[]
    {
                    new ChatMessage(ChatMessageRole.User, "🌎 Earth + 🌎 Earth"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🏔️ Mountain\" }"),
                    new ChatMessage(ChatMessageRole.User, "💧 Water + 💧 Water"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌊 Lake\" }"),
                    new ChatMessage(ChatMessageRole.User, "🔥 Fire + 🔥 Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌋 Volcano\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌬️ Wind + 🌬️ Wind"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌪️ Tornado\" }"),
                    new ChatMessage(ChatMessageRole.User, "💧 Water + 🔥 Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"💨 Steam\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌎 Earth + 🔥 Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌋 Lava\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌬️ Wind + 🔥 Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"💨 Smoke\" }"),
                    new ChatMessage(ChatMessageRole.User, "💧 Water + 🌊 Lake"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌊 Ocean\" }"),
                    new ChatMessage(ChatMessageRole.User, "💧 Water + 🌎 Earth"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌱 Plant\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌬️ Wind + 🌎 Earth"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌫️ Dust\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌫️ Dust + 🌱 Plant"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌱 Pollen\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌫️ Dust + 🌫️ Dust"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🏖️ Sand\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌱 Plant + 🌱 Plant"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌱 Tree\" }"),
                    new ChatMessage(ChatMessageRole.User, "🏔️ Mountain + 🏔️ Mountain"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🏔️ Mountain Range\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌫️ Dust + 🌎 Earth"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌎 Planet\" }"),
                    new ChatMessage(ChatMessageRole.User, "🏖️ Sand + 🔥 Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🪟 Glass\" }"),
                    new ChatMessage(ChatMessageRole.User, "🪟 Glass + 🔥 Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🔍 Lens\" }"),
                    new ChatMessage(ChatMessageRole.User, "🔍 Lens + 🔍 Lens"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🔭 Telescope\" }"),
                    new ChatMessage(ChatMessageRole.User, "🔭 Telescope + 🔥 Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"☀️ Sun\" }"),
                    new ChatMessage(ChatMessageRole.User, "☀️ Sun + 🐊 Swamp"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🦎 Lizard\" }"),
                    new ChatMessage(ChatMessageRole.User, "🦎 Lizard + ☀️ Sun"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🐉 Dragon\" }"),
                    new ChatMessage(ChatMessageRole.User, "🦎 Lizard + 🌊 Tsunami"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🦖 Godzilla\" }"),
                    new ChatMessage(ChatMessageRole.User, "🪵 Wood + 🌊 Ocean"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🚤 Boat\" }"),
                    new ChatMessage(ChatMessageRole.User, "🏖️ Sand + 🏖️ Sand"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🏜️ Desert\" }"),
                    new ChatMessage(ChatMessageRole.User, "🏜️ Desert + 🏖️ Sand"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🏜️ Desert\" }"),
                    new ChatMessage(ChatMessageRole.User, "💧 Water + 🌊 Ocean"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌊 Sea\" }"),
                    new ChatMessage(ChatMessageRole.User, "💧 Water + 🌊 Sea"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌊 Ocean\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌊 Ocean + 🌊 Sea"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌊 Ocean\" }"),
                    new ChatMessage(ChatMessageRole.User, "🏔️ Mountain Range + 🏔️ Mountain Range"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🏔️ Mountain Range\" }"),
                    new ChatMessage(ChatMessageRole.User, "🦖 Godzilla + 🐊 Swamp"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🐲🏞️ Swampzilla\" }"),
                    new ChatMessage(ChatMessageRole.User, "💨 Steam + 🧱 Brick"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🚂 Steam Engine\" }"),
                    new ChatMessage(ChatMessageRole.User, "🚂 Steam Engine + 🌳 Tree"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🚢 Steamboat\" }"),
                    new ChatMessage(ChatMessageRole.User, "🌱 Plant + 🌱 Plant"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌳 Tree\" }"),
                    new ChatMessage(ChatMessageRole.User, "♀️ Venus + 🌋 Volcano"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🖖 Vulcan\" }"),
                    new ChatMessage(ChatMessageRole.User, "🔭 Astronomer + 🖖 Vulcan"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🖖 Star Trek\" }"),
                    new ChatMessage(ChatMessageRole.User, "🧑‍🚀 Captain Krik + 🖖 Vulcan"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🖖 Spock\" }"),
                    new ChatMessage(ChatMessageRole.User, "🪵 Wood + 🖖 Vulcan"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🔥 Fire\" }"),
                    new ChatMessage(ChatMessageRole.User, "🛳️ Ship + 🧊 Iceberg"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🛳️ Titanic\" }"),
                    new ChatMessage(ChatMessageRole.User, "🧊 Iceberg + 🍵 Tea"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🍵 Ice Tea\" }"),
                    new ChatMessage(ChatMessageRole.User, "🧱 Brick + 🪐 Planet"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🪐 Mars\" }"),
                    new ChatMessage(ChatMessageRole.User, "🪐 Mars + 💩 Mud"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🪐 Mars\" }"),
                    new ChatMessage(ChatMessageRole.User, "🪐 Mars + 🔭 Telescope"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🪐 Mars\" }"),
                    new ChatMessage(ChatMessageRole.User, "🪐 Mars + 🌫️ Dust"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🐶 Rover\" }"),
                    new ChatMessage(ChatMessageRole.User, "🪐 Mars + ☀️ Sun"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌌 Solar System\" }"),
                    new ChatMessage(ChatMessageRole.User, "🔭 Telescope + 🍦 Ice Cream"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"result\": \"🌌 Galileo\" }"),
                    new ChatMessage(ChatMessageRole.System, "###"),
                    new ChatMessage(ChatMessageRole.System, "You are a helpful assitant that always " +
                    "responds in JSON in the form {\"result\": \"\"}" +
                    "Use the examples ABOVE the ### marker to help determine the combination of the " +
                    "elements. Make sure you always include an " +
                    "emoji(s) (no more than 2 emojis) representing the resulting element/entity. What is the result of combining " +
                    "the elements BELOW the --- marker?"),
                    new ChatMessage(ChatMessageRole.System, "---"),
                    new ChatMessage(ChatMessageRole.User, $"{one} + {two}")
    }
            };

            var tries = 0;
            InfinichemyData? results = null;
            while (tries < 3 && results == null)
            {
                try
                {
                    var response = await _openai.Chat.CreateChatCompletionAsync(chatRequest);
                    if (response != null)
                        results = JsonConvert.DeserializeObject<InfinichemyData>(response.ToString());
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

            if (results == null) throw new Exception("combination filed");

            return results.Result;
        }
    }
}
