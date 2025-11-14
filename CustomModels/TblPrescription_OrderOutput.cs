namespace MedicineSystemAPI.CustomModels
{
    public class TblPrescription_OrderOutput
    {
        public string? RefNumber { get; set; }

        public string? PatientName { get; set; }

        public string Doctor { get; set; }

        public string Description { get; set; } = null!;

        public decimal? TotalPrice { get; set; }

        public string Status { get; set; }

        public string? Note { get; set; }
        public List<TblMedicine_OrderOut> Medicines { get; set; }
    }
}
