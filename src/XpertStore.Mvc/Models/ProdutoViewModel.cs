using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XpertStore.Mvc.Models;

public class ProdutoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }


    [Required(ErrorMessage = "Descrição obrigatório")]
    [DisplayName("Descrição")]
    public string Descricao { get; set; }

    public string? Imagem { get; set; }

    [NotMapped]
    [DisplayName("Imagem do Produto")]
    public IFormFile? ImagemUpload { get; set; }

    [Required(ErrorMessage = "Preço obrigatório")]
    [DisplayName("Preço")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "Estoque obrigatório")]
    public int Estoque { get; set; }

    public Guid CategoriaId { get; set; }
}
