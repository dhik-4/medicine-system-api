namespace MedicineSystemAPI.CustomModels
{
    public class TblPrescription_OrderInput
    {
        public string? PatientName { get; set; }

        public int? DoctorsId { get; set; }

        public string Description { get; set; } = null!;

        public string? Note { get; set; }
        public List<TblMedicine_Order> Medicines { get; set; }
    }
}
