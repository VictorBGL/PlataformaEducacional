using PlataformaEducacional.Aluno.Application.AutoMapper;
using PlataformaEducacional.Api.Configurations;
using PlataformaEducacional.Conteudo.Application.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddApiConfig(builder.Configuration);
builder.Services.AddDependencyConfig();
builder.Services.AddIdentityConfig(builder.Configuration);
builder.Services.AddAutoMapper(typeof(ConteudoMapperConfig));
builder.Services.AddAutoMapper(typeof(AlunoMapperConfig));

var app = builder.Build();

app.UseApiConfig(app.Environment);

app.UseDbMigrationHelper();

app.Run();
