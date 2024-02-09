using HotChocolate.Data;
using Mug.Services.CosmosDb.Models;
using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models.ChooseGame;
using HotChocolate.Authorization;
using Mug.Services.CosmosDb.Models.Chat;

namespace Mug.Query
{
    public partial class Query
    {
        [UseSorting(Scope = "cosmos")]
        [UseFiltering(Scope = "cosmos")]
        public IExecutable<ChatMessage> Chat([Service] CosmosDbService cosmos)
        {
            return cosmos.Chat.AsExecutable();
        }
    }
}
