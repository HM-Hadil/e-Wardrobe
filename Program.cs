using e_commercedotNet.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services aux conteneurs
builder.Services.AddControllersWithViews();

// Ajouter le support pour la session
builder.Services.AddDistributedMemoryCache(); // Requis pour la gestion des sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Durée de timeout de la session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurer le DbContext pour utiliser SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection_toDb")));

// Ajouter le support pour HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configurer l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Chemin pour rediriger les utilisateurs non connectés
        options.AccessDeniedPath = "/Account/AccessDenied"; // Chemin pour accès refusé
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Durée de la session
    });

var app = builder.Build();

// Configurer le pipeline des requêtes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Ajouter l'authentification et l'autorisation
app.UseAuthentication();
app.UseAuthorization();

// Ajouter le middleware de session
app.UseSession();

// Configurer les routes par défaut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();