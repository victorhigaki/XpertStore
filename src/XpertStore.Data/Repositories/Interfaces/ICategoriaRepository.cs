using XpertStore.Data.Models;

namespace XpertStore.Data.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Task<List<Categoria>> GetAll();
    Task<Categoria?> GetById(Guid id);
}