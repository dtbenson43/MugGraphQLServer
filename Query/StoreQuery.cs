using HotChocolate.Data;
using MongoDB.Driver;
using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models;

namespace Mug.Query
{
    public partial class Query
    {
        [UseFirstOrDefault]
        public async Task<TestDocument> GetTestDocument([Service] CosmosDbService cosmos)
        {
            return await cosmos.TestDocument().Find(t => t.Type == "test").FirstOrDefaultAsync();
        }

    }
}
