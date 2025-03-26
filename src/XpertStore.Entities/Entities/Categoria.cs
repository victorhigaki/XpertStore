using System.ComponentModel.DataAnnotations;
using XpertStore.Domain.Entities.Base;

namespace XpertStore.Domain.Entities;

public class Categoria : BaseEntity
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Descricao { get; set; }
}
