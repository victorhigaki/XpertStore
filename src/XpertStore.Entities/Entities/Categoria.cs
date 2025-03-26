using System.ComponentModel.DataAnnotations;
using XpertStore.Entities.Models.Base;

namespace XpertStore.Entities.Models;

public class Categoria : BaseEntity
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Descricao { get; set; }
}
