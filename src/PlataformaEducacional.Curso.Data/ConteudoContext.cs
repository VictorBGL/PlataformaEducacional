using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Conteudo.Domain;
using PlataformaEducacional.Core.Data;

namespace PlataformaEducacional.Conteudo.Data
{
    public class ConteudoContext : DbContext, IUnitOfWork
    {
        public ConteudoContext(DbContextOptions<ConteudoContext> options)
            : base(options) { }

        public DbSet<PlataformaEducacional.Conteudo.Domain.Curso> Cursos { get; set; }
        public DbSet<Aula> Aulas { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
        //        e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
        //        property.Relational().ColumnType = "varchar(100)";

        //    modelBuilder.Ignore<Event>();

        //    modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
        //}

        public async Task<bool> Commit()
        {
            //foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            //{
            //    if (entry.State == EntityState.Added)
            //    {
            //        entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            //    }

            //    if (entry.State == EntityState.Modified)
            //    {
            //        entry.Property("DataCadastro").IsModified = false;
            //    }
            //}

            return await base.SaveChangesAsync() > 0;
        }
    }
}
