using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Mug.Services.CosmosDb;
using Newtonsoft.Json;

namespace Mug.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CosmosController : ControllerBase
    {
        private readonly CosmosDbService _cosmosDbService;

        public CosmosController(CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("cosmostest")]
        public async Task<string> Get()
        {
            var collection = _cosmosDbService.Conversations;
            var documents = await collection.Find(_ => true).ToListAsync();
            //return documents.ToJson(new MongoDB.Bson.IO.JsonWriterSettings { Indent = true });
            return JsonConvert.SerializeObject(documents, Formatting.Indented);
        }
    }
}
