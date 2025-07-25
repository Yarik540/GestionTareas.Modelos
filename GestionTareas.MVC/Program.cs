using GestionTareas.API.Consumer;
using GestionTareas.Modelos;

internal class Program
{
    private static void Main(string[] args)
    {

        Crud<Proyecto>.EndPoint = "https://localhost:7264/api/Proyectos";
        Crud<Usuario>.EndPoint = "https://localhost:7264/api/Usuarios";
        Crud<Tarea>.EndPoint = "https://localhost:7264/api/Tareas";

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

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

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}