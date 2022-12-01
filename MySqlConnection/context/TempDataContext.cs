using Microsoft.EntityFrameworkCore;

namespace MySqlConnectiondemo.context
{
    public class TempDataContext : DbContext
    {
        
        public TempDataContext(DbContextOptions<TempDataContext> options) : base(options) { }
        public DbSet<Temp> Temp;
    }
    public class Temp
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
