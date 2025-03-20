using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using UserAdmin.Core.Service;
using UserAdmin.Core.Service.Interface;
using UserAdmin.Infrastructure.Model;
using UsersAdmin.Client.Pages;
using UsersAdmin.Components; 
using UsersAdmin.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
 
var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();

// Register the DbContext with the open connection
builder.Services.AddDbContext<AdminUserDbContext>(options =>
    options.UseSqlite(connection));

builder.Services.AddScoped<AdminUserService>();
 

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7008/");
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AdminUserDbContext>();
    db.Database.EnsureCreated(); // Create the schema
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(UsersAdmin.Client._Imports).Assembly);

app.Run();
