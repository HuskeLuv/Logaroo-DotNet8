using Microsoft.EntityFrameworkCore;
using post.Models;

namespace app.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
    }
}