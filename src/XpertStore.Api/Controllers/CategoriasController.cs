using XpertStore.Api.Controllers.Base;
using XpertStore.Data.Data;
using XpertStore.Entities.Models;

namespace XpertStore.Api.Controllers;

public class CategoriasController : BaseCrudController<Categoria>
{
    public CategoriasController(ApplicationDbContext context) : base(context)
    {
    }
}
