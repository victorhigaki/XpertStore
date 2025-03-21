using Microsoft.EntityFrameworkCore;

namespace XpertStore.Mvc.Data
{
    public class SqliteDbContext : ApplicationDbContext
    {
        public SqliteDbContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnectionLite"));
        }
    }
}
