using backend.Middlewares;
using Core.MappingProfiles;
using Core.Services;
using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] [{SourceContext}] {Message}{NewLine}{Exception}";

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console(outputTemplate: logTemplate));

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
builder.Services.AddDbContext<UsersContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionStrings:UsersDB"]));
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<PermissionsService>();

builder.Services.AddScoped<DataSeeder>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Data Migrate/Seed

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
    if(context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }

    var dataSeader = scope.ServiceProvider.GetService<DataSeeder>();
    dataSeader.Seed();
}

#endregion

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
