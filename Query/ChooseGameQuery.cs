using HotChocolate.Data;
using Mug.Services.CosmosDb.Models;
using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models.ChooseGame;

namespace Mug.Query
{
    public partial class Query
    {
        [UseSorting(Scope = "cosmos")]
        [UseFiltering(Scope = "cosmos")]
        public IExecutable<ChooseGame> ChooseGames([Service] CosmosDbService cosmos)
        {
            return cosmos.ChooseGames.AsExecutable();
        }
    }
}
