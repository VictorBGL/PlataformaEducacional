using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Api.Entities;
using PlataformaEducacional.Core.Messages;

namespace PlataformaEducacional.Api.Data
{
    public class DbContext : IdentityDbContext<IdentityUser>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.Entity<Administrador>()
                        .ToTable(nameof(Administrador));

            modelBuilder.Entity<Administrador>()
                       .HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
