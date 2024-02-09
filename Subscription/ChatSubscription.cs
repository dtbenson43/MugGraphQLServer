using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using Mug.Services.CosmosDb.Models.Chat;

namespace Mug.Subscription
{
    public partial class Subscription
    {
        [Subscribe]
        [Topic($"{{{nameof(channel)}}}")]
        public ChatMessage ChatMessageAdded(string channel, [EventMessage] ChatMessage message)
        {
            return message;
        }
    }
}
