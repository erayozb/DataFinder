using Microsoft.EntityFrameworkCore;

namespace DataFinder.Wpf.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UrlSearch> UrlSearch { get; set; }
    }
}