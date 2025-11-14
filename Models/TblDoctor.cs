using System;
using System.Collections.Generic;

namespace MedicineSystemAPI.Models;

public partial class TblDoctor
{
    public int DoctorId { get; set; }

    public string Name { get; set; } = null!;

    public string Speciality { get; set; } = null!;

    public string Sip { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Note { get; set; }
}
