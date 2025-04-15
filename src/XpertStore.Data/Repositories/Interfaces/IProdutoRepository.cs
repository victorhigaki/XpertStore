using XpertStore.Data.Models;

namespace XpertStore.Data.Repositories.Interfaces;

public interface IProdutoRepository
{
    Task<List<Produto>> GetProdutosByUserId(Guid UserId);
    Task<Produto?> GetById(Guid Id);
    Task Create(Produto produto);
    Task Update(Produto produto);
    Task Delete(Produto produto);
    bool ProdutoExists(Guid id);
}