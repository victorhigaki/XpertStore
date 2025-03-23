using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XpertStore.Mvc.Models;

public class CategoriaViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Descrição obrigatório")]
    [DisplayName("Descrição")]
    public string Descricao { get; set; }
}
