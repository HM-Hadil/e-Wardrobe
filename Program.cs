using e_commercedotNet.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services aux conteneurs
builder.Services.AddControllersWithViews();

// Configurer le DbContext pour utiliser SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection_toDb")));

// Configurer l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Chemin pour rediriger les utilisateurs non connect�s
        options.AccessDeniedPath = "/Account/AccessDenied"; // Chemin pour acc�s refus�
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Dur�e de la session
    });

var app = builder.Build();

// Configurer le pipeline des requ�tes HTTP
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

// Configurer les routes par d�faut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
