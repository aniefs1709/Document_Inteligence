using DocumentIntelligence.Identity.Application.Abstractions;
using DocumentIntelligence.Identity.Application.Auth.Login;
using DocumentIntelligence.Identity.Application.Auth.Register;
using DocumentIntelligence.Identity.Infrastructure.Persistence;
using DocumentIntelligence.Identity.Infrastructure.Repositories;
using DocumentIntelligence.Identity.Infrastructure.Security;
using DocumentIntelligence.Identity.Infrastructure.Sessions;

using Microsoft.EntityFrameworkCore;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Read the Identity service's SQL Server connection string.
var connectionString =
    builder.Configuration.GetConnectionString("IdentityDatabase")
    ?? throw new InvalidOperationException(
        "Connection string 'IdentityDatabase' is missing.");

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration["Valkey:Configuration"]
        ?? throw new InvalidOperationException(
            "Valkey configuration is missing.");

    options.InstanceName =
        builder.Configuration["Valkey:InstanceName"]
        ?? "DocumentIntelligence:";
});

// Register EF Core with SQL Server.
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register Identity application and infrastructure services.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, IdentityPasswordHasher>();
builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(
        typeof(RegisterUserCommand).Assembly));
builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(
        typeof(LoginUserCommand).Assembly));
var valkeyConfiguration =
    builder.Configuration["Valkey:Configuration"]
    ?? throw new InvalidOperationException(
        "Valkey configuration is missing.");

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(valkeyConfiguration));

builder.Services.AddScoped<ISessionStore, ValkeySessionStore>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
