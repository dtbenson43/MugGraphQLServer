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

namespace Mug.Services.OpenAI
{

    public partial class OpenAIService
    {
        private readonly string _newGamePrompt = "START NEW GAME\n" +
            "TITLE: Stranded in Space\n" +
            "PLOT: The user is a duty officer of a small crew that includes the Captain " +
            "and 4 other crew members. The ship is on a colony mission with all of the " +
            "colonists on board in hyper sleep. The crew's " +
            "objective is to get the colonists to the destination planet unharmed. The ship is " +
            "very distant from any other ship or habitable planet. The crew has been travelling " +
            "for quite some time and some crew members are on edge and often confrontational and " +
            "this trait of the crew often appears when they're under stress. " +
            "Unbeknownst to the crew, a deadly alien life form has infiltrated the spacecraft. The alien " +
            "is very fast, very aggressive and very cunning. The alien, while intelligent, " +
            "cannot interact with electronics. The alien can not be contained " +
            "or reasoned with.";

        /*
         *                 "1. You are an experienced writer/director and you will be writing " +
                "content for a choose your own adventure game.\r\n2. Your output will " +
                "only be in JSON in the form { \"Text\": \"\", \"FirstOption\": \"\", \"SecondOption\":\"\" }." +
                "\r\n3. Describe every action, spoken dialogue, and " +
                "emotion in real-time as they happen in real-time.\r\n4. Each piece " +
                "of dialogue should be presented as it is **spoken** and should be " +
                "accompanied by detailed descriptions of tone, facial expressions, " +
                "and any tiny gestures or movements.\r\n5. Engage the five senses " +
                "Engaged. Use sensory language to describe the scene in intricate " +
                "detail—sight, sound, smell, touch, and taste should all be invoked i" +
                "n your descriptions.\r\n6. Emphasize the non-verbal cues, eye " +
                "movements, small smiles, and pauses that fill the conversation.\r\n7. " +
                "End every response in a way that keeps the scene open and demands " +
                "further expansion.\r\n8. No Summaries Allowed: Do not condense, " +
                "paraphrase, draw conclusions, foreshadow, or summarize anything. " +
                "Avoid general statements that summarize. Focus solely on describing " +
                "the unfolding events, actions, and emotions as they occur. " +
                "Dialogues should be presented as they are spoken.\r\n9. Be aware of " +
                "the pacing and tone of the story. Ensure the main plot isn't entered" +
                "until the characters and settings are established. take your time to " +
                "develop the characters " +
                "and setting. Don't just jump right into the action, but at the same" +
                "time, don't take too long. It's a difficult task, I know, but you can " +
                "do it." +
                "10. Consider what the reader could know when crafting the story. For " +
                "instance the tone story should not reflect fear or danger until the " +
                "danger is actually revealed." +
                "\r\n11. Write the longest detailed " +
                "response you can.\r\n\r\nThe " +
                "content ABOVE the ### are previous generated branches along with " +
                "user choices and previous plot and propmt information. Use it as " +
                "context for your writing. The content BELOW the --- marker are " +
                "additional context you should take into account when crafting the next " +
                "branch."
         */

        public async Task<ConsistencyVerificationData> VerifyConsistency(string aiResponse)
        {
            ChatRequest chatRequest = new ChatRequest()
            {
                Model = "gpt-3.5-turbo-1106",
                TopP = 0.9,
                Temperature = 0.2,
                MaxTokens = 4096,
                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                Messages = new ChatMessage[]
                {
                    new ChatMessage(ChatMessageRole.User, aiResponse),
                    new ChatMessage(ChatMessageRole.System, "###"),
                    new ChatMessage(ChatMessageRole.System, "You are a helpful assitant that always " +
                    "responds in JSON in the form {\"Consistent\": \"yes\", \"Reasoning\": \"\"}. " +
                    "Consistency should only be 'Yes' or 'No'. Reasoning is your reason for finding " +
                    "the response consistent or not. Is the response ABOVE the ### consistent with " +
                    "the instruction BELOW the --- marker?"),
                    new ChatMessage(ChatMessageRole.System, "---"),
                    new ChatMessage(ChatMessageRole.User, _newGamePrompt)
                }
            };

            var tries = 0;
            ConsistencyVerificationData? results = null;
            while (tries < 3 && results == null)
            {
                try
                {
                    var response = await _openai.Chat.CreateChatCompletionAsync(chatRequest);
                    if (response != null)
                        results = JsonConvert.DeserializeObject<ConsistencyVerificationData>(response.ToString());
                    if (results != null && (results.Consistent != "Yes" && results.Consistent != "No"))
                    {
                        throw new Exception("Consistent property is malformed");
                    }
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

            return results ?? new ConsistencyVerificationData()
            {
                Consistent = "Yes"
            };
        }
        public async Task<ChooseGameBranchData> CreateChooseGameBranchAsync(ChooseGame? game = null)
        {
            var messages = CreateChooseGameChatMessages(game);
            ChatRequest chatRequest = new ChatRequest()
            {
                Model = "gpt-4-turbo-preview", //"gpt-3.5-turbo-1106",
                TopP = 0.9,
                Temperature = 0.2,
                MaxTokens = 4096,
                ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                Messages = messages
            };

            var tries = 0;
            ChooseGameBranchData? results = null;
            while (tries < 3 && results == null)
            {
                try
                {
                    var response = await _openai.Chat.CreateChatCompletionAsync(chatRequest);
                    if (response != null)
                        results = JsonConvert.DeserializeObject<ChooseGameBranchData>(response.ToString());
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

            if (results != null && results.Text != null)
            {
                var verify = await VerifyConsistency(results.Text);
                if (verify != null && verify.Consistent != null && verify.Consistent == "No")
                {
                    List<ChatMessage> secondMessages = new List<ChatMessage>(messages);
                    if (secondMessages != null)
                    {
                        secondMessages.Add(new ChatMessage(ChatMessageRole.User, "The response is not consistent with the prompt."));
                        secondMessages.Add(new ChatMessage(ChatMessageRole.User, verify.Reasoning));
                        ChatRequest secondChatRequest = new ChatRequest()
                        {
                            Model = "gpt-4-turbo-preview", //"gpt-3.5-turbo-1106",
                            TopP = 0.9,
                            Temperature = 0.2,
                            MaxTokens = 4096,
                            ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
                            Messages = secondMessages
                        };

                        var secondTries = 0;
                        ChooseGameBranchData? secondResults = null;
                        while (secondTries < 3 && secondResults == null)
                        {
                            try
                            {
                                var secondResponse = await _openai.Chat.CreateChatCompletionAsync(secondChatRequest);
                                if (secondResponse != null)
                                    secondResults = JsonConvert.DeserializeObject<ChooseGameBranchData>(secondResponse.ToString());
                            }
                            catch (Exception)
                            {
                                if (secondTries == 2) throw;
                            }
                            finally
                            {
                                secondTries += 1;
                            }
                        }
                        results = secondResults ?? results;
                    }
                }
            }

            return results ?? new ChooseGameBranchData();
        }

        private ChatMessage[] CreateChooseGameChatMessages(ChooseGame? game)
        {
            var messages = new List<ChatMessage>
            {
                new(ChatMessageRole.System, _newGamePrompt)
            };

            ChooseGameBranch? currentBranch = null;
            while (game != null && ChooseGameUtilities.GetNextBranch(game, currentBranch, out currentBranch))
            {
                if (currentBranch == null) break;
                messages.Add(new(
                    ChatMessageRole.Assistant,
                    $"\nBRANCH TEXT: {currentBranch.Text}" +
                    $"\nOPTION ONE: {currentBranch.FirstOption.Text}" +
                    $"\nOPTION TWO: {currentBranch.SecondOption.Text}")
                );

                var userChoice = ChooseGameUtilities.GetUserChoice(currentBranch);
                var userChoiceOption = currentBranch.UserChoice;

                if (userChoiceOption == null || userChoice == null)
                    throw new Exception("User hasn't made a selection yet!");

                messages.Add(new(
                    ChatMessageRole.User,
                    $"\nUser Choice: {userChoiceOption} - {userChoice.Text}"));
            }
            messages.Add(new(ChatMessageRole.System, "###"));
            messages.Add(new(ChatMessageRole.System,
                "You are an experienced writer/director for a choose your own " +
                "adventure game. Your task is to craft content in real-time, " +
                "focusing on vivid sensory details, rich character expressions, " +
                "and nuanced dialogue. Remember to balance intricate descriptions " +
                "with a compelling pace to keep the narrative engaging. As you " +
                "write, pay attention to the unfolding events, ensuring that " +
                "every action, spoken word, and emotional nuance is captured " +
                "in the moment. Each dialogue should be detailed, reflecting " +
                "tone, facial expressions, and subtle gestures. Engage the five " +
                "senses in your descriptions, bringing the scene to life. Ensure " +
                "your responses leave the scene open for further expansion. The " +
                "story should unfold naturally, with characters and settings " +
                "established before delving into the main plot. Be mindful of " +
                "the reader's perspective, revealing information, including " +
                "danger, only when it becomes apparent in the story. The " +
                "narrative output should be in JSON format, in the form of " +
                "{ \"Text\": \"\", \"FirstOption\": \"\", \"SecondOption\":\"\" }. The choices " +
                "should be interactive, meaningful, and reflect the consequences " +
                "of past player decisions. Avoid summaries and generalizations. " +
                "Keep the narrative immediate and immersive, allowing players " +
                "to feel a part of the story as it unfolds. Be aware of pacing, " +
                "ensuring that details enrich rather than hinder the storytelling " +
                "experience." +
                "\r\n\r\n" +
                "The content ABOVE the ### are previous generated branches along with " +
                "user choices and previous plot and propmt information. Use it as " +
                "context for your writing. The content BELOW the --- marker are " +
                "additional context you should take into account when crafting the next " +
                "branch."
            ));
            messages.Add(new(ChatMessageRole.System, "---"));
            return [.. messages];
        }
    }
}
