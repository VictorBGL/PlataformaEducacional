using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Data;
using PlataformaEducacional.Api.Data;
using PlataformaEducacional.Api.Entities;
using PlataformaEducacional.Conteudo.Data;
using PlataformaEducacional.Financeiro.Data;
using System;

namespace PlataformaEducacional.Api.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<Context>();
            await context.Database.MigrateAsync();

            var conteudoContext = scope.ServiceProvider.GetRequiredService<ConteudoContext>();
            await conteudoContext.Database.MigrateAsync();

            var alunoContext = scope.ServiceProvider.GetRequiredService<AlunoContext>();
            await alunoContext.Database.MigrateAsync();

            var financeiroContext = scope.ServiceProvider.GetRequiredService<FinanceiroContext>();
            await financeiroContext.Database.MigrateAsync();


            await EnsureSeedProducts(context, alunoContext);
        }

        public static async Task EnsureSeedProducts(Context context, AlunoContext alunoContext)
        {
            #region Administrador

            if (context.Users.Any())
                return;

            var userId = Guid.NewGuid();

            await context.Users.AddAsync(new IdentityUser
            {
                Id = userId.ToString(),
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEJfnkXwMwUa7tr3NITeoGPnAjCbvkd5y2TdQ/wglcpCinkNGSF30kpgTIH3WsCTCkg==", // Teste@123
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            await context.SaveChangesAsync();

            if (context.Roles.Any())
                return;

            var roleId = Guid.NewGuid().ToString();

            await context.Roles.AddAsync(new IdentityRole
            {
                Id = roleId,
                Name = "Admin"
            });

            await context.SaveChangesAsync();

            if (context.UserRoles.Any())
                return;

            await context.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = userId.ToString()
            });

            await context.SaveChangesAsync();

            DateTime dataNascimentoAdmin = new DateTime(2000, 1, 1);

            var admin = new Administrador(userId, "Administrador", "admin@admin.com", dataNascimentoAdmin);

            context.Set<Administrador>().Add(admin);
            await context.SaveChangesAsync();

            #endregion

            #region Aluno

            var userAlunoId = Guid.NewGuid();

            await context.Users.AddAsync(new IdentityUser
            {
                Id = userAlunoId.ToString(),
                UserName = "aluno@aluno.com",
                NormalizedUserName = "ALUNO@ALUNO.COM",
                Email = "aluno@aluno.com",
                NormalizedEmail = "ALUNO@ALUNO.COM",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEJfnkXwMwUa7tr3NITeoGPnAjCbvkd5y2TdQ/wglcpCinkNGSF30kpgTIH3WsCTCkg==",  // Teste@123
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            await context.SaveChangesAsync();

            var roleAlunoId = Guid.NewGuid().ToString();

            await context.Roles.AddAsync(new IdentityRole
            {
                Id = roleAlunoId,
                Name = "Aluno"
            });

            await context.SaveChangesAsync();

            await context.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                RoleId = roleAlunoId,
                UserId = userAlunoId.ToString()
            });

            await context.SaveChangesAsync();

            DateTime dataNascimentoAluno = new DateTime(2000, 1, 1);

            var aluno = new Aluno.Domain.Aluno(userAlunoId, "Aluno", "aluno@aluno.com", dataNascimentoAluno);

            await alunoContext.Alunos.AddAsync(aluno);
            await context.SaveChangesAsync();

            #endregion
        }
    }
}
