using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models.Infinichemy;

namespace Mug.TypeExtensions
{
    [ExtendObjectType(typeof(InfCombination))]
    public class InfCombinationExtensions
    {
        public async Task<InfElement> GetResultElement([Parent] InfCombination com, [Service] CosmosDbService cosmos)
        {
            return await cosmos.GetInfElementByIdAsync(com.ResultElementId);
        }
    }
}
