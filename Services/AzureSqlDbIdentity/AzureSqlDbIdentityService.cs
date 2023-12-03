using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mug.Services.AzureSqlDbIdentity
{
    public class AzureSqlDbIdentityService : IdentityDbContext<IdentityUser>
    {
        public AzureSqlDbIdentityService(DbContextOptions<AzureSqlDbIdentityService> options)
            : base(options)
        {
        }
    }
}
