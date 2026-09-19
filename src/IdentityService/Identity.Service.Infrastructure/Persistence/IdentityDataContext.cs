using Identity.Service.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Service.Infrastructure.Persistence;

public class IdentityDataContext(DbContextOptions<IdentityDataContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    
}