using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Proyecto.Logica;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor
builder.Services.AddControllersWithViews();

// Configura el soporte para sesiones
builder.Services.AddDistributedMemoryCache(); // Soporte para caché en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible a través de HTTP
    options.Cookie.IsEssential = true; // La sesión es esencial para la aplicación
});
builder.Services.AddHttpClient();
// Registra Funciones como un servicio singleton
builder.Services.AddSingleton<Funciones>(sp =>
{
    var client = new MongoClient("mongodb://localhost:27017"); // Ajusta la cadena de conexión
    return new Funciones(client);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
