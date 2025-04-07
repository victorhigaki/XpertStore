using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XpertStore.Entities.Models;

namespace XpertStore.Data.Configurations;
public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Nome)
            .IsRequired();

        builder.Property(b => b.Descricao)
            .IsRequired();

        builder.Property(b => b.Preco)
            .IsRequired();

        builder.Property(b => b.Estoque)
            .IsRequired();

        builder.Property(b => b.Categoria)
            .IsRequired();

        builder.Property(b => b.Vendedor)
            .IsRequired();
    }
}
