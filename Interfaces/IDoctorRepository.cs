using MedicineSystemAPI.CustomModels;
using MedicineSystemAPI.Models;

namespace MedicineSystemAPI.Interfaces
{
    public interface IDoctorRepository
    {
        Task<List<TblDoctor>> GetDoctors(string doctorName, string SIP, CancellationToken cancellationToken);
        Task<bool> AddDoctors(TblDoctor input, CancellationToken cancellationToken);
        Task<bool> UpdateDoctors(TblDoctor data, CancellationToken cancellationToken);
    }
}
