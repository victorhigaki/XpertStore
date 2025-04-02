using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XpertStore.Data.Data;
using XpertStore.Data.Repositories.Interfaces;
using XpertStore.Entities.Models;

namespace XpertStore.Data.Repositories;
internal class ProdutoRepository : IProdutoRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public ProdutoRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<Produto>> GetProdutos(IdentityUser? user)
    {
        List<Produto> produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Vendedor)
            .ToListAsync();
        if (user != null)
            produtos.Where(p => p.VendedorId == new Guid(user.Id));
        return produtos;
    }
}
