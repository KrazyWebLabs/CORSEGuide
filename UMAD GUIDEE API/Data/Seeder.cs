using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShareProject.Models;
using ShareProject.Models.Enums;

namespace UMAD_GUIDEE_API.Data;

/// <summary>
/// Clase de seeder encargada de insertar datos iniciales en la base de datos.
/// </summary>
public class Seeder
{
    /// <summary>
    /// Metodo encargador de crear, administrar y manipular los datos que se usaran en la base de datos
    /// </summary>
    /// <param name="roleManager"></param>
    /// <param name="userManager"></param>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    public static async Task Seed(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, DataContext dataContext)
    {
        if ( !dataContext.Roles.Any() )
            await AddRoleAsync(roleManager, dataContext);

        if ( !dataContext.Users.Any() )
            await AddUserAsync(userManager, dataContext);

        if (!dataContext.Categories.Any() )
            await AddCategoriesAsync(dataContext);

        await dataContext.SaveChangesAsync();


        if ( !dataContext.Notes.Any() )
        {
            var worker = await dataContext.Workers.FirstOrDefaultAsync();
            var category = await dataContext.Categories.FirstOrDefaultAsync();

            await AddNotesAsync(worker, category, dataContext);
        }
        
        await dataContext.SaveChangesAsync();
    }

    /// <summary>
    /// Metodo encargado de crear las categorias que se usaran para las notas
    /// </summary>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    private static async Task AddCategoriesAsync(DataContext dataContext)
    {
        Category cat1 = new()
        {
            Name = "Licenciatura"
        };

        Category cat2 = new()
        {
            Name = "Maestria"
        };

        await dataContext.Categories.AddAsync(cat1);
        await dataContext.Categories.AddAsync(cat2);

        await dataContext.SaveChangesAsync();
    }

    /// <summary>
    /// Metodo encargado de agregar los roles que se declararon en el enum de 'ShareProject'
    /// </summary>
    /// <param name="roleManager"></param>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, DataContext dataContext)
    {
        foreach ( var role in Enum.GetValues<Roles>() )
        {
            if ( !await roleManager.RoleExistsAsync(role.ToString()) )
            {
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
        }
    }

    /// <summary>
    /// Metodo donde se crear los usuarios importantes, el administrador y el profesor
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    private static async Task AddUserAsync(UserManager<User> userManager, DataContext dataContext)
    {
        var user = await userManager.FindByEmailAsync("");
        var user2 = await userManager.FindByEmailAsync("");

        if ( user == null )
        {
            user = new()
            {
                Name = "Admin",
                LastName = "1",
                Email = "admin@mail.com",
                UserName = "admin@mail.com",
            };
        }

        if ( user2 == null )
        {
            user2 = new()
            {
                Name = "Pilar",
                LastName = "Cortes",
                Email = "pilar.cortes@mail.com",
                UserName = "pilar.cortes@mail.com",
            };
        }



        var result = await userManager.CreateAsync(user, "Admin123!");
        var result2 = await userManager.CreateAsync(user2, "pilar123");


        if ( result.Succeeded )
        {
            await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
        }

        if ( result2.Succeeded )
        {
            await userManager.AddToRoleAsync(user2, Roles.Teacher.ToString());
        }

        Worker worker = new()
        {
            User = user2
        };

        await dataContext.Workers.AddAsync(worker);
    }

    /// <summary>
    /// Metodo encargado de crear una nota de practica
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="category"></param>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    private static async Task AddNotesAsync(Worker? worker, Category? category, DataContext dataContext)
    {
        Note note = new()
        {
            Title = "Notaichon 1",
            Description = "Descripcion de Nota 1",
            Worker = worker,
            Catergory = category
        };


        await dataContext.Notes.AddAsync(note);
    }
}
