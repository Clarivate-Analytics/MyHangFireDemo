using Microsoft.EntityFrameworkCore;

namespace MyHangFireDemo.Helpers
{
    public class SQLDataContext : DbContext
    {
        public SQLDataContext(DbContextOptions<SQLDataContext> options)
        : base(options)
        {
        }

        public virtual DbSet<SQLBooks> Books { get; set; }
    }
}
