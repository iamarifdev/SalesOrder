namespace SalesOrder.Common.DTO.Element;

public class SubElementDto
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int WindowId { get; set; }

    public int Element { get; set; }

    public string Type { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}