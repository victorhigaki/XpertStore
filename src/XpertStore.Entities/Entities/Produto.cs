using System.ComponentModel.DataAnnotations;
using XpertStore.Entities.Models.Base;

namespace XpertStore.Entities.Models;

public class Produto : BaseEntity
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Descricao { get; set; }

    public string Imagem { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public int Estoque { get; set; }

    public Guid CategoriaId { get; set; }
    public Categoria Categoria { get; set; }

    public Guid VendedorId { get; set; }
    public Vendedor Vendedor { get; set; }
}
