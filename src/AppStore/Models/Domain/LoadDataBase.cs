using Microsoft.AspNetCore.Identity;
namespace AppStore.Models.Domain;

public class LoadDataBase
{
    public static async Task InsertarData(DataBaseContext context, 
                                        UserManager<ApplicationUser> userManagermanager, 
                                        RoleManager<IdentityRole> roleManager
                                        )
    {
        if(!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("ADMIN"));
        }

        if(!userManagermanager.Users.Any())
        {
            var usuario = new ApplicationUser {
                Nombre = "Cristian Omar",
                Email = "omar@correo.com",
                UserName = "cris.omar"
            };
            await userManagermanager.CreateAsync(usuario, "AndroideHD12#");
            await userManagermanager.AddToRoleAsync(usuario, "ADMIN");
        }

        if(!context.Categorias!.Any())
        {
            await context.Categorias!.AddRangeAsync(
                new Categoria { Nombre = "Drama" },
                new Categoria { Nombre = "Comedia" },
                new Categoria { Nombre = "Accion" },
                new Categoria { Nombre = "Terror" },
                new Categoria { Nombre = "Aventura" }
            );

            await context.SaveChangesAsync();
        }

        if(!context.Libros!.Any())
        {
            await context.Libros!.AddRangeAsync(
                new Libro { Titulo = "El Quijote de la Mancha", CreateDate = "06/06/2020", Imagen = "quijote.jpg", Autor = "Miguel de Cervantes" },
                new Libro { Titulo = "Harry Potter", CreateDate = "06/06/2020", Imagen = "harry.jpg", Autor = "Juan de la Vega" }
            );

            await context.SaveChangesAsync();
        }

        if(!context.LibroCategorias!.Any())
        {
            await context.LibroCategorias!.AddRangeAsync(
                new LibroCategoria { CategoriaId = 1, LibroId = 1 },
                new LibroCategoria { CategoriaId = 1, LibroId = 2 }
            );

            await context.SaveChangesAsync();
        }
    }
}