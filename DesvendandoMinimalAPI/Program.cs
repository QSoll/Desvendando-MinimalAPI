using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DesvendandoMinimalAPI.Context;
using DesvendandoMinimalAPI.Domain;
using DesvendandoMinimalAPI.DTOs;
using DesvendandoMinimalAPI.Services;
using System.Security.Claims;
using DesvendandoMinimalAPI.Repositories;
using DesvendandoMinimalAPI.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura a porta para rodar localmente e permitir acesso externo
builder.WebHost.UseUrls("http://0.0.0.0:5000");

// 🔧 Swagger com autenticação JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Serviços e autenticação
builder.Services.AddSingleton<AdministradorRepository>();
builder.Services.AddSingleton<TokenService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdmPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Adm"));
});

var app = builder.Build();

// Swagger em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de segurança
app.UseAuthentication();
app.UseAuthorization();

// Endpoints públicos
app.MapGet("/", () => "Boas vindas ao projeto DESVENDANDO MINIMAL API !!!");
app.MapGet("/ping", () => Results.Ok("API está funcionando !!!"));

//  Endpoints de administrador
app.MapPost("/administradores", (AdministradorCreateDTO dto, AdministradorRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(dto.Nome) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Senha))
        return Results.BadRequest("Nome, Email e Senha são obrigatórios.");

    var novoAdm = new Administrador
    {
        Nome = dto.Nome,
        Email = dto.Email,
        Senha = dto.Senha,
        Perfil = dto.Perfil
    };

    repo.Adicionar(novoAdm);

    return Results.Created($"/administradores/{novoAdm.Id}", novoAdm);
})
.WithName("CadastrarAdministrador");

app.MapGet("/administradores", (AdministradorRepository repo) =>
{
    return Results.Ok(repo.Listar());
})
.RequireAuthorization("AdmPolicy")
.WithName("ListarAdministradores");

// Perfil autenticado
app.MapGet("/perfil", (ClaimsPrincipal user) =>
{
    var email = user.FindFirst(ClaimTypes.Email)?.Value;
    var perfil = user.FindFirst(ClaimTypes.Role)?.Value;

    return Results.Ok(new
    {
        mensagem = "Você está autenticado!",
        email,
        perfil
    });
})
.RequireAuthorization()
.WithName("PerfilAutenticado");

// Login
app.MapPost("/login", (LoginDTO login, TokenService tokenService, AdministradorRepository repo) =>
{
    var adm = repo.BuscarPorEmailSenha(login.Email, login.Senha);

    if (adm is null)
        return Results.Unauthorized();

    var token = tokenService.GerarToken(adm.Email, adm.Perfil);

    return Results.Ok(new
    {
        token,
        administrador = new
        {
            adm.Id,
            adm.Nome,
            adm.Email,
            adm.Perfil
        }
    });
})
.WithName("LoginAdministrador");

// Endpoints de veículos
app.MapVeiculoEndpoints();

app.Run();

// Necessário para testes
public partial class Program { }
