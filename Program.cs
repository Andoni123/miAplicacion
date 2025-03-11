using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddRazorPages();
builder.Services.AddSingleton<CarritoService>();

// 1. Agregar caché distribuida (requerido para la sesión)
builder.Services.AddDistributedMemoryCache();

// 2. Configurar la sesión
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".miAplicacion.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 3. Agregar autenticación
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Index";
        options.LogoutPath = "/Logout";
    });

// Agregar esto ANTES de builder.Build()
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// El orden de estos middleware es crítico
app.UseRouting();

// IMPORTANTE: UseSession debe ir ANTES de UseAuthentication
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();