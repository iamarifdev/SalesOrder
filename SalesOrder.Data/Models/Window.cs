using System.ComponentModel.DataAnnotations;

namespace SalesOrder.Data.Models;

public class Window : BaseModel<int>
{
    [Required] public int OrderId { get; set; }

    [Required] [StringLength(255)] public string Name { get; set; }

    [Required] [Range(0, int.MaxValue)] public int QuantityOfWindows { get; set; }

    [Required] [Range(0, int.MaxValue)] public int TotalSubElements { get; set; }

    public virtual Order Order { get; set; }

    public virtual ICollection<SubElement> SubElements { get; set; }
}