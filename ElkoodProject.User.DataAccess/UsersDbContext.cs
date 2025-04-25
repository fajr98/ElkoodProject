namespace ElkoodProject.User.DataAccess;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class UsersDbContext : IdentityDbContext
{

    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }
}
