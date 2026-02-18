using AuthGuardCore.Context;
using AuthGuardCore.Entities;
using AuthGuardCore.Interfaces;
using AuthGuardCore.Models;
using AuthGuardCore.Models.JwtModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthGuardCoreContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthGuardCoreContext>()
    .AddErrorDescriber<CustomIdentityValidator>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/Error/403";
});

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<JwtSettingsModel>(
    builder.Configuration.GetSection("JwtSettingsKey"));


builder.Services.AddAuthentication()
    // JWT (API için)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtSettings = builder.Configuration
            .GetSection("JwtSettingsKey")
            .Get<JwtSettingsModel>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    })

   // Google External Login (Web için - Cookie ile çalışır)
   .AddGoogle(options =>
   {
       options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
       options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
   });

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
