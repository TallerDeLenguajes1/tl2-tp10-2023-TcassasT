using tl2_tp10_2023_TcassasT.Interfaces;
using tl2_tp10_2023_TcassasT.Middlewares;
using tl2_tp10_2023_TcassasT.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

string databaseConectionString = builder.Configuration.GetConnectionString("SqliteConexion");
builder.Services.AddSingleton<string>(databaseConectionString);

builder.Services.AddScoped<ITableroReposiroty, TableroRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioTableroRepository, UsuarioTableroRepository>();
builder.Services.AddScoped<IActividadRepository, ActividadRepository>();

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
app.UseSession();
app.UseAuthorization();

// Middleware para verificar datos en session, importante que esté luego del useSession
app.UseMiddleware<VerificarDatosEnSesion>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
