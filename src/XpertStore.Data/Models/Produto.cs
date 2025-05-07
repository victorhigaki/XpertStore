using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using XpertStore.Data.Models.Base;

namespace XpertStore.Data.Models;

public class Produto : BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Preencha o campo {0}!")]
    public string Nome { get; set; }


    [Required(ErrorMessage = "Preencha o campo {0}!")]
    [DisplayName("Descrição")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "Preencha o campo {0}!")]
    public string? Imagem { get; set; }

    [DisplayName("Preço")]
    [Required(ErrorMessage = "Preencha o campo {0}!")]
    [Range(0.01, double.MaxValue, ErrorMessage = "{0} precisa ser maior que {1}.")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "Preencha o campo {0}!")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} precisa ser maior que {1}.")]
    public int Estoque { get; set; }

    public Categoria Categoria { get; set; }

    public Vendedor Vendedor { get; set; }
}
