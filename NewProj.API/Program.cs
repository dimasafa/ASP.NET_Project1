using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NewProj.API.Mappings;
using NewProj.API.Repositories;
using NZWalks.API.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NewProj.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Microsoft.AspNetCore.Diagnostics;
using NewProj.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    // Logs in File(Path bis File und interval, nachdem wird einen neuen File erstellt)
    .WriteTo.File("Logs/NzWalks.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddControllers();
// Add actuelle Path von API (notwendig um die Path bis Images zu erhalten.)
builder.Services.AddHttpContextAccessor();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Erweiterung von swagger, um jwt token zu testieren 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {апрап
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<NZWalksDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString"));
});

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString"));
});


// регистрируем службу SQLRegionRepository как реализацию интерфейса IRegionRepository. AddScoped - регистрирует службу в контейнере зависимостей
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

// anmeldung vom Automapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Identifikation. Registrieren <IdentityUser> und wahlen weiter die Einstellungen.
builder.Services.AddIdentityCore<IdentityUser>()
    // добавляем поддержку ролей в процессе идентифиукации
    .AddRoles<IdentityRole>()
    // Добавляем провайдера для токена для системы идентификации. "NZWalks" - имя, которое будет использоваться для этого провайдера
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    // используем NZWalksAuthDbContext для хранения данных идентификации. NZWalksAuthDbContext - это контекст базы данных, который будет использоваться для хранения информации о пользователях и ролях.
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    // Метод для генерации токенов(стандартные провайдеры для операций с токенами, такие как создание и проверка их действительности.)
    .AddDefaultTokenProviders();

// Identifakation for Admin
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configuraton von Password (diese Configuration konnete selbst gewählt kann)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


// Authentification mit jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Logger
app.UseMiddleware<ExceptionHandlerMiddlware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Addieren die Möglichkeit mit static file zu arbeiten (zb. Images, html usw.)
app.UseStaticFiles(new StaticFileOptions
{
    // Addieren die Path, wo werden wir Images nehmen
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
