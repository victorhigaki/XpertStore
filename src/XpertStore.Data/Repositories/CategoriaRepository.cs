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

    public async Task<Categoria?> GetById(Guid? id)
    {
        if (id == null)
        {
            return null;
        }
        return await _context.Categorias.FindAsync(id);
    }

    public async Task Create(Categoria model)
    {
        _context.Add(model);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Categoria model)
    {
        _context.Update(model);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Categoria categoria)
    {
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
        }

        await _context.SaveChangesAsync();
    }

    public bool Exists(Guid? id)
    {
        return _context.Categorias.Any(e => e.Id == id);
    }
}
