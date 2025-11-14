using System;
using System.Collections.Generic;

namespace MedicineSystemAPI.Models;

public partial class TblMedicine
{
    public int MedicineId { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public int? Stock { get; set; }

    public decimal? Price { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? Note { get; set; }
}
