using ChatOperaMini;
using ChatOperaMini.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var accessMdbFile = builder.Configuration.GetConnectionString("DefaultConnectionJet")!.Split('=');
var accessMdbPath = Path.Combine(builder.Environment.ContentRootPath, accessMdbFile[1]);
#pragma warning disable CA1416 // Validate platform compatibility
builder.Services.AddDbContext<AppDbContext>(option => option.UseJet($"{accessMdbFile[0]}={accessMdbPath}")); 
#pragma warning restore CA1416 // Validate platform compatibility

builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Run automatic migration
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}");

app.MapControllerRoute(
    name: "custom",
    pattern: "{sender}/{channelCode=public}/{operaMini=0}",
    defaults: new { controller = "Chat", action = "Index" });

app.MapHub<ChatHub>("/chathub");

app.Run();
