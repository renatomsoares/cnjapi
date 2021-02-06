using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contexts
{
    public class CNJDBContext : DbContext
    {
        public CNJDBContext(DbContextOptions<CNJDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lawsuit>(entity =>
            {
                entity.ToTable("Lawsuit");
                entity.HasKey(e => e.IdLawsuit);
                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            });
        }
    }
}

