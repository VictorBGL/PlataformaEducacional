using MediatR;
using PlataformaEducacional.Aluno.Data;
using PlataformaEducacional.Aluno.Domain;
using PlataformaEducacional.Core.Communication.Mediator;
using PlataformaEducacional.Core.Communication;
using PlataformaEducacional.Core.Messages.CommonMessages.Notifications;
using PlataformaEducacional.Financeiro.Data.Repository;
using PlataformaEducacional.Financeiro.Domain;
using PlataformaEducacional.Conteudo.Domain;
using PlataformaEducacional.Conteudo.Data;
using PlataformaEducacional.Conteudo.Application.Interfaces;
using PlataformaEducacional.Conteudo.Application.Services;
using PlataformaEducacional.Conteudo.Domain.Interfaces;
using PlataformaEducacional.Conteudo.Domain.Services;
using PlataformaEducacional.Aluno.Application.Queries;
using PlataformaEducacional.Aluno.AntiCorruption;
using PlataformaEducacional.Aluno.Application.Commands;

namespace PlataformaEducacional.Api.Configurations
{
    public static class DependencyConfig
    {
        public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IConteudoAppService, ConteudoAppService>();
            services.AddScoped<ICursoService, CursoService>();
            services.AddScoped<ICursoRepository, CursoRepository>();

            services.AddScoped<IAlunoQueries, AlunoQueries>();
            services.AddScoped<IConteudoService, ConteudoService>();
            services.AddScoped<IAlunoRepository, AlunoRepository>();

            services.AddScoped<IRequestHandler<FinalizarAulaCommand, bool>, AlunoCommandHandler>();
            services.AddScoped<IRequestHandler<EmitirCertificadoCommand, bool>, AlunoCommandHandler>();
            services.AddScoped<IRequestHandler<RealizarMatriculaCommand, bool>, AlunoCommandHandler>();

            services.AddScoped<IFinanceiroRepository, FinanceiroRepository>();

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            return services;
        }
    }
}
