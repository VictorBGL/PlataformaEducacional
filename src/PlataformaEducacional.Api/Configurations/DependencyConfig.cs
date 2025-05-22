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

namespace PlataformaEducacional.Api.Configurations
{
    public static class DependencyConfig
    {
        public static IServiceCollection AddDependencyConfig(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IAlunoRepository, AlunoRepository>();

            //services.AddScoped<IConteudoAppService, ConteudoAppService>();
            //services.AddScoped<IConteudoService, ConteudoService>();
            services.AddScoped<ICursoRepository, CursoRepository>();

            services.AddScoped<IFinanceiroRepository, FinanceiroRepository>();

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            return services;
        }
    }
}
