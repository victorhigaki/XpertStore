using XpertStore.Data.Data;
using XpertStore.Data.Models;
using XpertStore.Data.Repositories.Interfaces;

namespace XpertStore.Data.Repositories;
public class VendedorRepository : IVendedorRepository
{
    private readonly ApplicationDbContext _context;

    public VendedorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Vendedor?> GetById(Guid UserId)
    {
        return await _context.Vendedores.FindAsync(UserId);
    }
}
