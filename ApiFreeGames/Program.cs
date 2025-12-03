using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ApiFreeGames.Services; // 👈 IMPORTANTE

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Leer datos del appsettings.json
var jwtKey = config["JwtSettings:Key"];
var issuer = config["JwtSettings:Issuer"];
var audience = config["JwtSettings:Audience"];

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHttpClient<GameService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
        {
            // ACEPTA EL CERTIFICADO AUNQUE ESTÉ MAL
            return true;
        }
    };
});

builder.Services.AddScoped<GameService>();

builder.Services.AddHttpClient<GameService>();
; // 👈 ESTE ES TU SERVICE

// Configurar JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTTOKENS API",
        Version = "v1"
    });

    // Definición del esquema Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "AGREGA el token manualmente en cada petición.\nFormato: Bearer <tu_token>",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Para que aparezcan los candaditos en Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();