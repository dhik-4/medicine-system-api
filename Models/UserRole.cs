using System;
using System.Collections.Generic;

namespace MedicineSystemAPI.Models;

public partial class UserRole
{
    public byte Id { get; set; }

    public string Name { get; set; } = null!;
}
