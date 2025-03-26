using System.ComponentModel.DataAnnotations;

namespace XpertStore.Domain.Entities.Base;
public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
