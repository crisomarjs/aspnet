using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Implementation;

public class LibroService : ILibroService
{
    private readonly DataBaseContext ctx;

    public LibroService(DataBaseContext ctxParameter)
    {
        ctx = ctxParameter;
    }

    public bool Add(Libro libro)
    {
        try
        {
            ctx.Libros!.Add(libro);
            ctx.SaveChanges();
            foreach(int categoriaId in libro.Categorias!)
            {
                var libroCategoria = new LibroCategoria
                {
                    LibroId = libro.Id,
                    CategoriaId = categoriaId
                };

                ctx.LibroCategorias!.Add(libroCategoria);
            }
            ctx.SaveChanges();
            return true;
        }
        catch(Exception )
        {
            return false;
        }
    }

    public bool Delete(int id)
    {
        try 
        {
            var data = GetById(id);
            if(data is null)
            {
                return false;
            }
            
            var libroCategorias = ctx.LibroCategorias!.Where(a => a.LibroId == data.Id);
            ctx.LibroCategorias!.RemoveRange(libroCategorias);
            ctx.Libros!.Remove(data);
            ctx.SaveChanges();
            return true;
        }
        catch(Exception )
        {
            return false;
        }
    }

    public Libro GetById(int id)
    {
        return ctx.Libros!.Find(id)!;
    }

    public LibroListVm List(string term = "", bool paging = false, int currentPage = 0)
    {
        var data = new LibroListVm();
        var list = ctx.Libros!.ToList();

        if(!string.IsNullOrEmpty(term))
        {
            term = term.ToLower();
            list = list.Where(a => a.Titulo!.ToLower().StartsWith(term)).ToList();
        }

        if(paging)
        {
            int pageSize = 5;
            int count = list.Count;
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            data.PageSize = pageSize;
            data.CurrentPage = currentPage;
            data.TotalPages = totalPages;
        }

        foreach(var libro in list)
        {
            var categorias = (
                from Categoria in ctx.Categorias!
                join lc in ctx.LibroCategorias! 
                on Categoria.Id equals lc.CategoriaId
                where lc.LibroId == libro.Id
                select Categoria.Nombre
            ).ToList();

            string categoriasNombres = string.Join(", ", categorias);
            libro.categoriasNames = categoriasNombres;
        }

        data.LibroList = list.AsQueryable();
        return data;
    }

    public bool Update(Libro libro)
    {
        try
        {
            var categoriasParaEliminar = ctx.LibroCategorias!.Where(a => a.LibroId == libro.Id);
            foreach(var categoria in categoriasParaEliminar)
            {
                ctx.LibroCategorias!.Remove(categoria);
            }

            foreach(int categoriaId in libro.Categorias!)
            {
                var libroCategoria = new LibroCategoria
                {
                    CategoriaId = categoriaId,
                    LibroId = libro.Id
                };

                ctx.LibroCategorias!.Add(libroCategoria);
            }

            ctx.Libros!.Update(libro);
            ctx.SaveChanges();
            return true;
        }
        catch(Exception )
        {
            return false;
        }
    }

    public List<int> GetCategoryByLibroId(int libroId)
    {
        return ctx.LibroCategorias!.Where(a => a.LibroId == libroId).Select(a => a.CategoriaId).ToList(); 
    }

}