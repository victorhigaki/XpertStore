using Microsoft.EntityFrameworkCore;
using XpertStore.Data.Data;
using XpertStore.Data.Models;
using XpertStore.Data.Repositories.Interfaces;

namespace XpertStore.Data.Repositories;
public class ProdutoRepository : IProdutoRepository
{
    private readonly ApplicationDbContext _context;

    public ProdutoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> GetProdutosByUserId(Guid UserId)
    {
        var produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Vendedor)
            .Where(p => p.Vendedor.Id == UserId)
            .ToListAsync();
        return produtos;
    }

    public async Task<Produto?> GetById(Guid id)
    {
         var produto = await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Vendedor)
            .FirstOrDefaultAsync(m => m.Id == id);
        return produto;
    }

    public async Task Create(Produto produto)
    {
        _context.Add(produto);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Produto produto)
    {
        _context.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Produto produto)
    {

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }

    public bool Exists(Guid id)
    {
        return _context.Produtos.Any(e => e.Id == id);
    }

    public async Task<bool> ProdutoIsUsingCategoria(Guid categoriaId)
    {
        return await _context.Produtos.AnyAsync(p => p.Categoria.Id == categoriaId);
    }
}

