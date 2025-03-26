using System.ComponentModel.DataAnnotations;

namespace XpertStore.Entities.Models.Base;
public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
