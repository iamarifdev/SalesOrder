using System.ComponentModel.DataAnnotations;

namespace SalesOrder.Data.Models;

public class SubElement : BaseModel<int>
{
    [Required] public int OrderId { get; set; }

    [Required] public int WindowId { get; set; }

    [Required] [Range(0, int.MaxValue)] public int Element { get; set; }

    [Required] [StringLength(50)] public string Type { get; set; }

    [Required] [Range(0, int.MaxValue)] public int Width { get; set; }

    [Required] [Range(0, int.MaxValue)] public int Height { get; set; }

    public virtual Order Order { get; set; }
    public virtual Window Window { get; set; }
}