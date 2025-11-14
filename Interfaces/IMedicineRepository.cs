using MedicineSystemAPI.Models;

namespace MedicineSystemAPI.Interfaces
{
    public interface IMedicineRepository
    {
        Task<List<TblMedicine>> GetMedicine(CancellationToken cancellationToken);
        Task<bool> AddMedicine(TblMedicine data, CancellationToken cancellationToken);
        Task<bool> UpdateMedicine(TblMedicine data, CancellationToken cancellationToken);
    }
}
