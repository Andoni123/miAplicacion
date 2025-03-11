using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddRazorPages();

// Agregar autenticación por cookies - Esta es la parte importante que faltaba
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.Cookie.Name = "miAplicacion.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Estos dos middleware son esenciales y deben ir en este orden
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run(); 