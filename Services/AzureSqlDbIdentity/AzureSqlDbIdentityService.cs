using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mug.Services.AzureSqlDbIdentity
{
    public class AzureSqlDbIdentityService(DbContextOptions<AzureSqlDbIdentityService> options) : IdentityDbContext<IdentityUser>(options)
    {
    }
}
