using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace Mug.Services.OpenAI
{
    public partial class OpenAIService
    {
        private OpenAIAPI _openai;
        public OpenAIService(string OpenAIKey)
        {
            _openai = new OpenAIAPI(OpenAIKey);
        }

        public OpenAIAPI Client { get { return _openai; } }

        public async Task<string> Chat(string prompt)
        {
            var result = await _openai.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo_16k,
                Temperature = 0.1,
                MaxTokens = 4096,
                Messages = new ChatMessage[] {
                new ChatMessage(ChatMessageRole.User, prompt)
            }});

            return result.ToString();
        }
    }
}
