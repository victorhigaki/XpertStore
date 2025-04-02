using Microsoft.EntityFrameworkCore;
using XpertStore.Business.Services.Interfaces;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;

namespace XpertStore.Business.Services;

public class ProdutoService : IProdutoService
{
    private readonly ApplicationDbContext _context;

    public ProdutoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> Get(Guid? userId)
    {
        if (userId == null)
        {
            return Enumerable.Empty<Produto>();
        }

        var res = await _context.Produtos
           .Include(p => p.Categoria)
           .Include(p => p.Vendedor)
           .Where(p => p.Vendedor.Id == userId)
           .ToListAsync();

        return res;
    }

}
