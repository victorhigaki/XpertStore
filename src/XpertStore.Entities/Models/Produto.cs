namespace XpertStore.Entities.Models;

public class Produto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Imagem { get; set; }
    public decimal Preco { get; set; }
    public bool Estoque { get; set; }
    public Guid CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
}
