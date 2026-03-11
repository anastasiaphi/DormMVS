using DormDomain.Model;
using DormInfrastructure;
using DormInfrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Do2Context>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IDataPortServiceFactory<Faculty>, FacultyServiceFactory>();
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
    // pattern: "{controller=Tariffs}/{action=Index}/{id?}")
    pattern: "{controller=Faculties}/{action=Index}/{id?}")
  //pattern: "{controller=Degrees}/{action=Index}/{id?}")

    .WithStaticAssets();


app.Run();
