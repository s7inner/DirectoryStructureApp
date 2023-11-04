using DirectoryStructureApp.Data;
using DirectoryStructureApp.Interfaces;
using DirectoryStructureApp.Repositories;
using DirectoryStructureApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// AddListCatalogs services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// AddListCatalogs services to the container.
builder.Services.AddRazorPages();

// ConnectionString
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration
    .GetConnectionString("AppConnectionString"))
    );

builder.Services.AddScoped<IMyCatalogRepository, MyCatalogRepository>();
builder.Services.AddScoped<IJsonFileService, JsonFileService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MyCatalogs}/{action=Index}/{id?}");

app.MapRazorPages();

//при першому запуску заповнює бд вхідними даними
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<CatalogDbContext>();
        await DataGenerator.InitCatalogAsync(serviceProvider); // зміни тут
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

app.Run();
