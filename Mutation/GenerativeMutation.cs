using Mug.Services.OpenAI;
using HotChocolate.Authorization;

using OpenAI_API.Chat;

namespace Mug.Mutation
{
    public partial class Mutation
    {
        [Authorize]
        public async Task<GetChatResponsePayload> GetChatResponse(string prompt, [Service] OpenAIService openAI)
        {
            var result = await openAI.Chat(prompt);
            return new GetChatResponsePayload()
            {
                chatResult = result
            };
        }
    }

    public class GetChatResponsePayload
    {
        public string? chatResult { get; set; }
    }
}
