using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace XpertStore.Entities.Models;

public class Categoria : BaseEntity
{
    [Required(ErrorMessage = "Preencha o campo {0}!")]
    public string Nome { get; set; }

    [DisplayName("Descrição")]
    [Required(ErrorMessage = "Preencha o campo {0}!")]
    public string Descricao { get; set; }
}
