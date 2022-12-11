//namespace MySqlPolemro.context

using Microsoft.EntityFrameworkCore;
using MySqlPolemro.Models;
using System.Collections.Generic;

namespace MySqlPolemro.context
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
