namespace XpertStore.Api.Models;

public class ProdutoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Imagem { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public CategoriaViewModel Categoria { get; set; }
    public VendedorViewModel Vendedor { get; set; }
}
