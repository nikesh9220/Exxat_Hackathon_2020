using Microsoft.EntityFrameworkCore;

namespace Hackathon.Models
{
    public class _DbContext : DbContext
    {
        public DbSet<StudentDetails> StudentDetails { get; set; }
        public DbSet<UserPreferences> userPreferences { get; set; }

        public _DbContext(DbContextOptions<_DbContext> options) : base(options)
        {

        }
    }
}
