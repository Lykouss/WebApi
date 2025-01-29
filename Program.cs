using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();

// Forma de autenticação.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
// Parâmetros de validação do token.
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Valida quem está solicitando.
        ValidateAudience = true, // Valida quem está recebendo.
        ValidateLifetime = true, // Define se o tempo de expiração será validado.
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chaveautenticacao")), // Chave de autenticação.
        ClockSkew = TimeSpan.FromMinutes(30), // Valida o tempo de expiração do token.
        ValidIssuer = "exoapi.webapi", // Nome do emissor.
        ValidAudience = "exoapi.webapi" // Nome do destinatário.
    };
});

builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
