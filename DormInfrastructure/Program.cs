using DormDomain.Model;
using DormInfrastructure;
using DormInfrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Do2Context>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));


builder.Services.AddScoped<IDataPortServiceFactory<Faculty>, FacultyDataPortServiceFactory>();
builder.Services.AddScoped<IImportService<Faculty>, FacultiesImportService>();

builder.Services.AddScoped<IExportService<Faculty>, FacultyExportService>();
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Faculties}/{action=Index}/{id?}")


    .WithStaticAssets();


app.Run();
