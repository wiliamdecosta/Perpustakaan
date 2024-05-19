using AutoMapper;
using DbUp;
using JustclickCoreModules.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Perpustakaan.Data;
using Perpustakaan.Data.Repositories.Implementation;
using Perpustakaan.Mappers;
using Perpustakaan.Middleware;
using Perpustakaan.Middleware.Exceptions;
using Perpustakaan.Services;
using Perpustakaan.Utils;
using Perpustakaan.Utils.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
configuration.AddEnvironmentVariables();

string connectionString = configuration["ConnectionStrings:DefaultPostgreConnection"]
    .Replace("__DB_SERVER__", configuration["DB_SERVER_PERPUS"])
    .Replace("__DB_PORT__", configuration["DB_PORT_PERPUS"])
    .Replace("__DB_NAME__", configuration["DB_NAME_PERPUS"])
    .Replace("__DB_USERNAME__", configuration["DB_USERNAME_PERPUS"])
    .Replace("__DB_PASSWORD__", configuration["DB_PASSWORD_PERPUS"]);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option => {
    //option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultPostgreConnection"));
    option.UseNpgsql(connectionString);
});


//DbUp execute
var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsFromFileSystem("DbUp_Migrations")
                .Build();
var result = upgrader.PerformUpgrade();
if (result.Successful)
{
    System.Diagnostics.Debug.WriteLine("DbUp Execute Success!");
}
else
{
    System.Diagnostics.Debug.WriteLine(result.Error.Message);
}

//Mapper config
IMapper mapper = MappingConfigs.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Endpoint Authorization
builder.Services.AddScoped<EndpointAuthorizationMiddleware>();
//Global Exception Handler
builder.Services.AddScoped<GlobalExceptionHandler>();

// Add services to the container
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<RequestValidator>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<EndpointRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<BookService>();

//filter,searching,sorting util
builder.Services.AddTransient(typeof(FilterUtil<>));

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddTransient<JwtUtil>();

builder.Services.AddAuthorization();

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

app.UseMiddleware<EndpointAuthorizationMiddleware>();
app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();

app.Run();
