using Microsoft.EntityFrameworkCore;
using XpertStore.Data.Data;
using XpertStore.Data.Models;
using XpertStore.Data.Repositories.Interfaces;

namespace XpertStore.Data.Repositories;
public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext _context;

    public CategoriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> GetAll()
    {
        List<Categoria> categorias = await _context.Categorias.ToListAsync();
        return categorias;
    }

    public async Task<Categoria?> GetById(Guid id)
    {
        return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
    }
}
