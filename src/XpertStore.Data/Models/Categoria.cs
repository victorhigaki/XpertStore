using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using XpertStore.Data.Models.Base;

namespace XpertStore.Data.Models;

public class Categoria : BaseEntity
{
    [Required(ErrorMessage = "Preencha o campo {0}!")]
    public string Nome { get; set; }

    [DisplayName("Descrição")]
    [Required(ErrorMessage = "Preencha o campo {0}!")]
    public string Descricao { get; set; }
}
