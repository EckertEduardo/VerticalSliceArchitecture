using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitecture.Entities;

namespace VerticalSliceArchitecture.DataBase
{
    public class MyContext : DbContext
    {
        public DbSet<Motorcycle> Motorcycles { get; set; }

        public MyContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Motorcycle>();
        }
    }
}
