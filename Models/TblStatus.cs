using System;
using System.Collections.Generic;

namespace MedicineSystemAPI.Models;

public partial class TblStatus
{
    public byte StatusId { get; set; }

    public string Description { get; set; } = null!;

    public string? Note { get; set; }
}
