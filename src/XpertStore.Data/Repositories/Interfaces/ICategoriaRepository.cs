using XpertStore.Data.Models;

namespace XpertStore.Data.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Task Create(Categoria model);
    Task Delete(Categoria categoria);
    bool Exists(Guid? id);
    Task<List<Categoria>> GetAll();
    Task<Categoria?> GetById(Guid? id);
    Task Update(Categoria model);
}