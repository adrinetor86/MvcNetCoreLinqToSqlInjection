using MvcNetCoreLinqToSqlInjection.Models;
using MvcNetCoreLinqToSqlInjection.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Nota, los Repos suelen ir como Transient

//builder.Services.AddTransient<RepositorySQLServer>();
// builder.Services.AddTransient<RepositoryDoctoresOracle>();

//builder.Services.AddTransient<IRepositoryDoctores,RepositorySQLServer>();
builder.Services.AddTransient<IRepositoryDoctores,RepositoryDoctoresOracle>();

Coche car= new Coche();
car.Marca = "Pontiac";
car.Modelo = "FIREBIRD";
car.Imagen = "coche.png";
car.Velocidad = 0;
car.VelocidadMaxima = 280;

builder.Services.AddSingleton<ICoche,Coche>(x=> car);



//RESOLVEMOS EL SERVICIO COCHE PARA LA INYECCION
//builder.Services.AddTransient<Coche>();
//builder.Services.AddSingleton<ICoche,Deportivo>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
