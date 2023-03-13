using ExtraSliceV2.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtraSliceV2.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options) { }
        DbSet<User> Users { get; set; }
    }
}
