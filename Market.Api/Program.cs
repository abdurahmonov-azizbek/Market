using Market.Api.Middlewares;
using Market.Application;
using Market.Data.DbContexts;
using Market.Domain.Enums;
using Market.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//db context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

//Configure setting models
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

//Add services
builder.Services.AddServices();

//Exposers
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

//Add context accessor
builder.Services.AddHttpContextAccessor();

//Swagger configuration
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter valid token"
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
            Array.Empty<string>()
        }
    });
});

//Add JWT
var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
    ?? throw new InvalidOperationException("Jwt settings is not configured!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = jwtSettings.ValidateIssuer,
            ValidIssuer = jwtSettings.ValidIssuer,
            ValidateAudience = jwtSettings.ValidateAudience,
            ValidAudience = jwtSettings.ValidAudience,
            ValidateLifetime = jwtSettings.ValidateLifeTime,
            ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            RoleClaimType = nameof(Role)
        };
    }
);

//Add cors
builder.Services.AddCors(options => options.AddPolicy("AllowSpecificOrigin", policy =>
{
    policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));



var app = builder.Build();

//Dev tools
app.UseSwagger();
app.UseSwaggerUI();

//Use cors
app.UseCors("AllowSpecificOrigin");

//Use custom middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

//Configure authentication
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();