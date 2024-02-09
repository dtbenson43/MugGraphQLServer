using HotChocolate.Subscriptions;
using Mug.Extensions;
using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models.Chat;
using Mug.Subscription;
using System.Security.Claims;

namespace Mug.Mutation
{
    public partial class Mutation
    {
        public async Task<AddChatMessagePayload> AddChatMessageAsync(
            string channel,
            string message,
            string name,
            ClaimsPrincipal claimsPrincipal,
            [Service] CosmosDbService cosmos,
            [Service] ITopicEventSender sender)
        {
            var chatMessage = new ChatMessage()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = claimsPrincipal.GetUserId() ?? "Anonymous",
                Name = name,
                Message = message,
                Channel = channel,
                DateTime = DateTime.UtcNow
            };

            await cosmos.AddChatMessageAsync(chatMessage);
            await sender.SendAsync(chatMessage.Channel, chatMessage);

            return new AddChatMessagePayload
            {
                AddedMessage = chatMessage,
            };
        }
    }

    public class AddChatMessagePayload
    {
        public ChatMessage AddedMessage { get; set; } = null!;
    }
}
