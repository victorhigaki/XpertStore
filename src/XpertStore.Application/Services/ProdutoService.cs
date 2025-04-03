using Microsoft.EntityFrameworkCore;
using XpertStore.Application.Services.Interfaces;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;

namespace XpertStore.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly ApplicationDbContext _context;

    public ProdutoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> GetAllAsync(Guid? userId)
    {
        if (userId == null)
        {
            return [];
        }

        var produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Vendedor)
            .Where(p => p.VendedorId == userId)
            .ToListAsync();

        return produtos;
    }

    public bool ProdutoExists(Guid id)
    {
        return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
