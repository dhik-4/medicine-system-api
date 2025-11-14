namespace MedicineSystemAPI.CustomModels
{
    public class TblMedicine_Input
    {
        public string Name { get; set; } = null!;

        public string? Code { get; set; }

        public int? Stock { get; set; }

        public decimal? Price { get; set; }

        public string? Note { get; set; }
    }
}
