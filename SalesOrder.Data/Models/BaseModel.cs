using System.ComponentModel.DataAnnotations;

namespace SalesOrder.Data.Models;

public class BaseModel<T> : IBaseModel<T> where T : struct
{
    public T Id { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}