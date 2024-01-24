using System.Security.Claims;

namespace Mug.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            // Check if the principal is null
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            // Find the claim with the specified key
            var claim = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            // Return the claim value or null if the claim is not found
            return claim?.Value;
        }
    }
}
