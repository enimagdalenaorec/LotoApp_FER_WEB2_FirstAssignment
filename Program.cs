using LotoApp.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Auth0.AspNetCore.Authentication;
using LotoApp.Repositories;
using LotoApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var username = Environment.GetEnvironmentVariable("DB_USERNAME");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var host = "localhost";
var port = "5433";
var dbname = "LotoDb";

var connectionString = $"Host={host};Port={port};Database={dbname};Username={username};Password={password}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN")!;
    options.ClientId = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID")!;
    options.ClientSecret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET")!;
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();
builder.Services.AddScoped<RoundService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<TicketService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
