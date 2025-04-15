using XpertStore.Data.Models;

namespace XpertStore.Data.Repositories.Interfaces;

public interface IVendedorRepository
{
    Task<Vendedor?> GetById(Guid UserId);
}