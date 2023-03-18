namespace SalesOrder.Data.Models;

public interface IBaseModel<T> where T : struct
{
    T Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}