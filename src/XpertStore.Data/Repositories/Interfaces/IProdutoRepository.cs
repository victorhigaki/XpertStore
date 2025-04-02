using Microsoft.AspNetCore.Identity;
using XpertStore.Entities.Models;

namespace XpertStore.Data.Repositories.Interfaces;

public interface IProdutoRepository
{
    Task<List<Produto>> GetProdutos(IdentityUser? user);
}