using System;
using System.Collections.Generic;

namespace MedicineSystemAPI.Models;

public partial class TblPrescription
{
    public long PrescriptionId { get; set; }

    public string? RefNumber { get; set; }

    public string? PatientName { get; set; }

    public string? Medicines { get; set; }

    public int? DoctorsId { get; set; }

    public string Description { get; set; } = null!;

    public decimal? TotalPrice { get; set; }

    public byte? StatusId { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
