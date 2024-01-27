using HotChocolate.Data;
using Mug.Services.CosmosDb.Models;
using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models.ChooseGame;
using HotChocolate.Authorization;

namespace Mug.Query
{
    public partial class Query
    {
        [UseSorting(Scope = "cosmos")]
        [UseFiltering(Scope = "cosmos")]
        [Authorize]
        public IExecutable<ChooseGame> ChooseGames([Service] CosmosDbService cosmos)
        {
            return cosmos.ChooseGames.AsExecutable();
        }
    }
}
