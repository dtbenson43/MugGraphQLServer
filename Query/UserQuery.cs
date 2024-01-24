using HotChocolate.Authorization;
using Mug.Extensions;
using System.Security.Claims;

namespace Mug.Query
{
    public class User
    {
        public List<string> Claims { get; set; } = null!;
        public string IdentityName { get; set; } = null!;
        public bool IsAuthenticated { get; set; }

        public User(IEnumerable<Claim> claims, ClaimsIdentity? identity)
        {
            Claims = claims.Select(c => $"{c.Type}: {c.Value}").ToList();
            IdentityName = identity?.Name ?? "Anonymous";
            IsAuthenticated = identity?.IsAuthenticated ?? false;
        }
    }

    public partial class Query
    {
        [Authorize]
        public string? GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.GetUserId();
        }
    }
}
