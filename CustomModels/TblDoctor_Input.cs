namespace MedicineSystemAPI.CustomModels
{
    public class TblDoctor_Input
    {
        public string Name { get; set; } = null!;

        public string Speciality { get; set; } = null!;

        public string Sip { get; set; } = null!;

        public string? Note { get; set; }
    }
}
