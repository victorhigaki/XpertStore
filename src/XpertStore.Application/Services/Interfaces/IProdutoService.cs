using XpertStore.Entities.Models;

namespace XpertStore.Application.Services.Interfaces;
public interface IProdutoService
{
    Task<IEnumerable<Produto>> GetAllAsync(Guid? userId);
    bool ProdutoExists(Guid id);
}
