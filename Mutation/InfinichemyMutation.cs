using HotChocolate.Subscriptions;
using Mug.Extensions;
using Mug.Services.CosmosDb;
using Mug.Services.CosmosDb.Models.Chat;
using Mug.Services.CosmosDb.Models.Infinichemy;
using Mug.Services.OpenAI;
using Mug.Subscription;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Mug.Mutation
{
    public partial class Mutation
    {
        public async Task<GetCombinationPayload> GetCombination(
            string one,
            string two,
            [Service] CosmosDbService cosmosDbService,
            [Service] OpenAIService openAI)
        {
            var trimmedOne = Regex.Replace(one, @"\s+", "");
            var trimmedTwo = Regex.Replace(two, @"\s+", "");

            string[] elements = { trimmedOne, trimmedTwo };
            Array.Sort(elements);

            var hash = CreateHash(string.Join("", elements));

            CombinationResult combinationResult = await cosmosDbService.GetCombinationResultByHashAsync(hash);

            if (combinationResult == null)
            {
                // If not found, use the AI service to generate a new combination
                var aiResult = await openAI.GetCombination(one, two); // Assuming this method returns a string result

                // Create and store the new combination result
                combinationResult = new CombinationResult
                {
                    Hash = hash,
                    ElementOne = elements[0],
                    ElementTwo = elements[1],
                    Result = aiResult
                };

                await cosmosDbService.AddCombinationResultAsync(combinationResult);
            }

            return new GetCombinationPayload
            {
                CombinationResult = combinationResult
            };
        }

        [GraphQLIgnore]
        private static string CreateHash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

    public class GetCombinationPayload
    {
        public CombinationResult CombinationResult { get; set; } = null!;
    }
}
