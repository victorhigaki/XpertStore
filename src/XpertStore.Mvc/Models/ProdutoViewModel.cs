using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace XpertStore.Mvc.Models;

public class ProdutoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }

    public string? Imagem { get; set; }

    [NotMapped]
    [DisplayName("Imagem do Produto")]
    public IFormFile? ImagemUpload { get; set; }

    public decimal Preco { get; set; }
    public bool Estoque { get; set; }
    public Guid CategoriaId { get; set; }

}
