using System.Collections.ObjectModel;
using Entities;

namespace Models.Models
{
    public class Produto
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Imagem { get; set; }
        public string? Preco { get; set; }
        public int Estoque { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
