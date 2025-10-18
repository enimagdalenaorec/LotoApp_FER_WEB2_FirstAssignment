using LotoApp.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Auth0.AspNetCore.Authentication;
using LotoApp.Repositories;
using LotoApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var username = Environment.GetEnvironmentVariable("DB_USERNAME");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var host = "localhost";
var port = "5433";
var dbname = "LotoDb";

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")  ?? $"Host={host};Port={port};Database={dbname};Username={username};Password={password}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddControllersWithViews();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN")!;
    options.ClientId = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID")!;
    options.ClientSecret = Environment.GetEnvironmentVariable("AUTH0_CLIENT_SECRET")!;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {           // for m2m auth0 flow for admin endpoints
        options.Authority = "https://dev-r72sm216mqp1z7np.us.auth0.com/";
        options.Audience = "https://lotoapi";
    });

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();
builder.Services.AddScoped<RoundService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<TicketService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
