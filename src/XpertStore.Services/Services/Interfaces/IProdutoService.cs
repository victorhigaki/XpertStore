using System.Security.Claims;
using XpertStore.Entities.Models;

namespace XpertStore.Business.Services.Interfaces;
public interface IProdutoService
{
    Task<IEnumerable<Produto>> Get(Guid? userId);
}
