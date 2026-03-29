using System;
using System.Collections.Generic;

namespace MedicineSystemAPI.Models;

public partial class User
{
    public int Userid { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public byte? IsActive { get; set; }

    public byte? RoleId { get; set; }

    public int? DoctorId { get; set; }

    public int? PharmacistId { get; set; }

    public string Password { get; set; } = null!;
}
