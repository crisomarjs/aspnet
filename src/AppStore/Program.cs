using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;
using AppStore.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IFileService, FileService>();

// Conection to the database
builder.Services.AddDbContext<DataBaseContext>(options => {
    options.LogTo(Console.WriteLine, new [] {
        DbLoggerCategory.Database.Command.Name},
        LogLevel.Information).EnableSensitiveDataLogging();

        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDataBase"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddDefaultTokenProviders();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var ambiente = app.Services.CreateAsyncScope())
{
    var services = ambiente.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<DataBaseContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();
        await LoadDataBase.InsertarData(context, userManager, roleManager);
    }
    catch(Exception ex)
    {
        var loggin = services.GetRequiredService<ILogger<Program>>();
        loggin.LogError(ex, "Error en la carga de datos");
    }
}

app.Run();
