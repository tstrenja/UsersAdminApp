using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAdmin.Infrastructure.Model
{
    public class AdminUserDbContextFactory : IDesignTimeDbContextFactory<AdminUserDbContext>
    {
        public AdminUserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdminUserDbContext>();
            optionsBuilder.UseSqlite("DataSource=:memory:");  

            return new AdminUserDbContext(optionsBuilder.Options);
        }
    }
    public class AdminUserDbContext :DbContext
    {
        public DbSet<User> Users { get; set; } 
        public DbSet<Logs> Logs { get; set; } 

        public AdminUserDbContext(DbContextOptions<AdminUserDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=:memory:");
            }
        }
    }
}
