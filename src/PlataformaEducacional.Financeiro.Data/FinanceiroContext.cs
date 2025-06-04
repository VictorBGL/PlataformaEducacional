using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Core.Data;
using PlataformaEducacional.Core.Messages;
using PlataformaEducacional.Financeiro.Domain;

namespace PlataformaEducacional.Financeiro.Data
{
    public class FinanceiroContext : DbContext, IUnitOfWork
    {
        public FinanceiroContext(DbContextOptions<FinanceiroContext> options)
            : base(options) { }

        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }

            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceiroContext).Assembly);
        }


        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return await base.SaveChangesAsync() > 0;
        }
    }
}
