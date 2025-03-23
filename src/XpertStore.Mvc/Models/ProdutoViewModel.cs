namespace XpertStore.Mvc.Models;

public class ProdutoViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Imagem { get; set; }
    public decimal Preco { get; set; }
    public bool Estoque { get; set; }
    public Guid CategoriaId { get; set; }

}
