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
using Mug.Services.CosmosDb.Models.Infinichemy;

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

        public async Task<InfElementData> GetInfCombination(string one, string two)
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
                    new ChatMessage(ChatMessageRole.User, "Earth + Earth"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Mountain\", \"Emoji\": \"🏔️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Water + Water"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Lake\", \"Emoji\": \"🌊\" }"),
                    new ChatMessage(ChatMessageRole.User, "Fire + Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Volcano\", \"Emoji\": \"🌋\" }"),
                    new ChatMessage(ChatMessageRole.User, "Wind + Wind"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Tornado\", \"Emoji\": \"🌪️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Water + Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Steam\", \"Emoji\": \"💨\" }"),
                    new ChatMessage(ChatMessageRole.User, "Earth + Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Lava\", \"Emoji\": \"🌋\" }"),
                    new ChatMessage(ChatMessageRole.User, "Wind + Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Smoke\", \"Emoji\": \"💨\" }"),
                    new ChatMessage(ChatMessageRole.User, "Water + Lake"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Ocean\", \"Emoji\": \"🌊\" }"),
                    new ChatMessage(ChatMessageRole.User, "Water + Earth"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Plant\", \"Emoji\": \"🌱\" }"),
                    new ChatMessage(ChatMessageRole.User, "Wind + Earth"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Dust\", \"Emoji\": \"🌫️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Dust + Plant"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Pollen\", \"Emoji\": \"🌱\" }"),
                    new ChatMessage(ChatMessageRole.User, "Dust + Dust"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Sand\", \"Emoji\": \"🏖️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Plant + Plant"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Tree\", \"Emoji\": \"🌳\" }"),
                    new ChatMessage(ChatMessageRole.User, "Mountain + Mountain"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Mountain Range\", \"Emoji\": \"🏔️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Dust + Earth"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Planet\", \"Emoji\": \"🌎\" }"),
                    new ChatMessage(ChatMessageRole.User, "Sand + Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Glass\", \"Emoji\": \"🪟\" }"),
                    new ChatMessage(ChatMessageRole.User, "Glass + Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Lens\", \"Emoji\": \"🔍\" }"),
                    new ChatMessage(ChatMessageRole.User, "Lens + Lens"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Telescope\", \"Emoji\": \"🔭\" }"),
                    new ChatMessage(ChatMessageRole.User, "Telescope + Fire"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Sun\", \"Emoji\": \"☀️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Sun + Swamp"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Lizard\", \"Emoji\": \"🦎\" }"),
                    new ChatMessage(ChatMessageRole.User, "Lizard + Sun"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Dragon\", \"Emoji\": \"🐉\" }"),
                    new ChatMessage(ChatMessageRole.User, "Lizard + Tsunami"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Godzilla\", \"Emoji\": \"🦖\" }"),
                    new ChatMessage(ChatMessageRole.User, "Wood + Ocean"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Boat\", \"Emoji\": \"🚤\" }"),
                    new ChatMessage(ChatMessageRole.User, "Sand + Sand"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Desert\", \"Emoji\": \"🏜️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Desert + Sand"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Desert\", \"Emoji\": \"🏜️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Water + Ocean"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Sea\", \"Emoji\": \"🌊\" }"),
                    new ChatMessage(ChatMessageRole.User, "Water + Sea"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Ocean\", \"Emoji\": \"🌊\" }"),
                    new ChatMessage(ChatMessageRole.User, "Ocean + Sea"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Ocean\", \"Emoji\": \"🌊\" }"),
                    new ChatMessage(ChatMessageRole.User, "Mountain Range + Mountain Range"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Mountain Range\", \"Emoji\": \"🏔️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Godzilla + Swamp"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Swampzilla\", \"Emoji\": \"🐲🏞️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Steam + Brick"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Steam Engine\", \"Emoji\": \"🚂\" }"),
                    new ChatMessage(ChatMessageRole.User, "Steam Engine + Tree"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Steamboat\", \"Emoji\": \"🚢\" }"),
                    new ChatMessage(ChatMessageRole.User, "Plant + Plant"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Tree\", \"Emoji\": \"🌳\" }"),
                    new ChatMessage(ChatMessageRole.User, "Venus + Volcano"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Vulcan\", \"Emoji\": \"🖖\" }"),
                    new ChatMessage(ChatMessageRole.User, "Astronomer + Vulcan"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Star Trek\", \"Emoji\": \"🖖\" }"),
                    new ChatMessage(ChatMessageRole.User, "Captain Kirk + Vulcan"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Spock\", \"Emoji\": \"🖖\" }"),
                    new ChatMessage(ChatMessageRole.User, "Wood + Vulcan"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Fire\", \"Emoji\": \"🔥\" }"),
                    new ChatMessage(ChatMessageRole.User, "Ship + Iceberg"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Titanic\", \"Emoji\": \"🛳️\" }"),
                    new ChatMessage(ChatMessageRole.User, "Iceberg + Tea"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Ice Tea\", \"Emoji\": \"🍵\" }"),
                    new ChatMessage(ChatMessageRole.User, "Brick + Planet"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Mars\", \"Emoji\": \"🪐\" }"),
                    new ChatMessage(ChatMessageRole.User, "Mars + Mud"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Mars\", \"Emoji\": \"🪐\" }"),
                    new ChatMessage(ChatMessageRole.User, "Mars + Telescope"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Mars\", \"Emoji\": \"🪐\" }"),
                    new ChatMessage(ChatMessageRole.User, "Mars + Dust"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Rover\", \"Emoji\": \"🐶\" }"),
                    new ChatMessage(ChatMessageRole.User, "Mars + Sun"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Solar System\", \"Emoji\": \"🌌\" }"),
                    new ChatMessage(ChatMessageRole.User, "Telescope + Ice Cream"),
                    new ChatMessage(ChatMessageRole.Assistant, "{ \"Result\": \"Galileo\", \"Emoji\": \"🌌\" }"),
                    new ChatMessage(ChatMessageRole.System, "###"),
                    new ChatMessage(ChatMessageRole.System, "Welcome to your role as an AI alchemist, " +
                    "where you will embark on an intriguing journey to uncover the mysteries of combining " +
                    "various elements. This task is not just about mixing elements but about discovering " +
                    "the essence of their interaction and the new entities they can create. As you explore " +
                    "and document your alchemical discoveries, please present your findings in a " +
                    "structured JSON format with the form { \"Result\": \"\", \"Emoji\": \"\" } to ensure " +
                    "clarity and consistency. Use the examples ABOVE the ### marker and follow these " +
                    "enriched guidelines to ensure your explorations are both profound and imaginative:\n\n" +
                    "Objective Clarification: Your mission is to combine elements, objects, concepts, or " +
                    "entities to unveil new outcomes. Reflect on how elements interact on a fundamental " +
                    "level and what unique results they can produce.\n\n" +
                    "Encourage Deeper Exploration: Push the boundaries of your creativity. Look for the " +
                    "less obvious, the surprising, and the profound. For example, what does combining " +
                    "\"🌪️ Air\" with \"🔥 Fire\" yield? Perhaps \"⚡ Lightning,\" showcasing the dynamic " +
                    "interaction between elements.\n\n" +
                    "Additional Examples for Inspiration:\n\n" +
                    "\"Water\" + \"Cold\" might logically lead to \"❄️ Ice,\" showing a transformation " +
                    "based on conditions.\n" +
                    "\"Time\" + \"Machine\" could intriguingly result in \"🕰️ Time Travel,\" a concept " +
                    "that extends beyond physical elements to theoretical possibilities.\n" +
                    "Probing Questions to Guide Your Thought Process:\n\n" +
                    "Does combining similar elements change their scale or create something new? E.g., " +
                    "\"Ocean\" + \"Ocean\" remains \"🌊 Ocean,\" respecting logical upper limits.\n" +
                    "How does the interaction between elements reflect a deeper theme or concept? For " +
                    "instance, \"Book\" + \"Magic\" might give rise to \"📜 Spell,\" combining knowledge with " +
                    "the mystical.\n" +
                    "Emphasize Emoji Accuracy: The Emoji you choose must directly represent the outcome of " +
                    "your combination. This is crucial for clear communication. If \"🌱 Seed\" and \"🌧️ " +
                    "Water\" create \"🌻 Sunflower,\" your Emoji selection must reflect the final product, not " +
                    "the process. Practically there will not be an ideal Emoji for every element so in cases " +
                    "where an ideal Emoji is not available prioritize matching the color of the Emoji to the " +
                    "color essence of the outcome.\n\n" +
                    "Reflect on Logical Upper and Lower Limits: Consider the magnitude and limits of your " +
                    "combinations. Does adding more of one element fundamentally change the outcome, or does it " +
                    "reach a saturation point where the essence remains the same?\n\n" +
                    "Consider Consequences and Broader Implications: Dive into the immediate and long-term " +
                    "effects of your combinations. How do they alter their environment or the dynamics between " +
                    "elements?\n\n" +
                    "Unleash Creativity Within Logical Frameworks: While logic is the foundation, your " +
                    "creativity is the key to unlocking new discoveries. Use probing questions to guide your " +
                    "imagination:\n\n" +
                    "What if an element is taken to its conceptual extreme?\n" +
                    "How does the context or combination redefine the elements involved?\n" +
                    "Directives for Result Representation:\n\n" +
                    "Ensure your outcomes are not transient states or temporary events. Aim for definitive " +
                    "results that encapsulate the essence of the combination.\n" +
                    "Avoid simplistic word combinations; strive for inventive and meaningful outcomes that " +
                    "resonate with the essence of the elements combined.\n" +
                    "By enriching your alchemical quest with these guidelines, you're not just combining elements " +
                    "but weaving together a tapestry of imagination, logic, and insight. Let each combination " +
                    "tell a story, reveal a truth, or open a door to unexplored realms. Remember, the most " +
                    "profound discoveries lie at the intersection of creativity and reasoning.\n\n" +
                    "Remember to respond in JSON with the form { \"Result\": \"\", \"Emoji\": \"\" }. What is the " +
                    "result of the combination BELOW the --- marker?"),
                    new ChatMessage(ChatMessageRole.System, "---"),
                    new ChatMessage(ChatMessageRole.User, $"{one} + {two}")
                }
            };

            var tries = 0;
            InfElementData? results = null;
            while (tries < 3 && results == null)
            {
                try
                {
                    var response = await _openai.Chat.CreateChatCompletionAsync(chatRequest);
                    if (response != null)
                        results = JsonConvert.DeserializeObject<InfElementData>(response.ToString());
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

            if (results == null) throw new Exception("combination failed");

            return results;
        }
    }
}
