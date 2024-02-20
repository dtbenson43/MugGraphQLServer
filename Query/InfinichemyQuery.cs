using HotChocolate.Data;
using Mug.Services.CosmosDb.Models;
using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models.ChooseGame;
using HotChocolate.Authorization;
using Mug.Services.CosmosDb.Models.Chat;
using Mug.Services.CosmosDb.Models.Infinichemy;

namespace Mug.Query
{
    public partial class Query
    {
        public async Task<InfCombination?> GetInfCombination(string combinationId, [Service] CosmosDbService cosmos)
        {
            return await cosmos.GetInfCombinationByIdAsync(combinationId);
        }
    }
}
