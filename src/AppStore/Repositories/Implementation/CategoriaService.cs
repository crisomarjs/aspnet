using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Implementation;

public class CategoriaService : ICategoriaService
{
    private readonly DataBaseContext ctx;

    public CategoriaService(DataBaseContext ctx)
    {
        this.ctx = ctx;
    }

    public IQueryable<Categoria> List()
    {
        return ctx.Categorias!.AsQueryable();
    }
}