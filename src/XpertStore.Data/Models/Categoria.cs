using XpertStore.Data.Models.Base;

namespace XpertStore.Data.Models;

public class Categoria : BaseEntity
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
