using HotChocolate.Data;
using MongoDB.Driver;
using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models;

namespace Mug.Query
{
    public partial class Query
    {
        [UseSorting(Scope = "cosmos")]
        [UseFiltering(Scope = "cosmos")]
        public IExecutable<ConversationMessage> Conversations([Service] CosmosDbService cosmos)
        {
            return cosmos.Conversations.AsExecutable();
        }

    }
}
