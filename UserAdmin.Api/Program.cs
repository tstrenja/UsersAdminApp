using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserAdmin.Core;
using UserAdmin.Core.Service;
using UserAdmin.Core.Service.Interface;
using UserAdmin.Infrastructure.Model;

var builder = WebApplication.CreateBuilder(args);

var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();

// Register the DbContext with the open connection
builder.Services.AddDbContext<AdminUserDbContext>(options =>options.UseSqlite(connection));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Users Admin API",
        Description = ".NET 8 Web API"
    });
});
builder.Services.AddControllers();
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AdminUserDbContext>();
    db.Database.EnsureCreated();  
}
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
