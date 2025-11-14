using MedicineSystemAPI.CustomModels;
using MedicineSystemAPI.Models;

namespace MedicineSystemAPI.Interfaces
{
    public interface IPrescriptionRepository
    {
        Task<TblPrescription_OrderOutput> AddPrescription(TblPrescription_OrderInput input, CancellationToken cancellationToken);
        Task<TblPrescription_OrderOutput> GetPrescription(string refNumber, string Name, CancellationToken cancellationToken);
        Task<List<TblStatus>> GetPrescriptionStatus(CancellationToken cancellationToken);
        Task<bool> PrescriptionStatusReserved(long Id, string RefNumber, CancellationToken cancellationToken);
        Task<bool> PrescriptionStatusInProgress(long Id, string RefNumber, CancellationToken cancellationToken);
        Task<bool> PrescriptionStatusFinished(long Id, string RefNumber, CancellationToken cancellationToken);
        Task<bool> PrescriptionStatusCompleted(long Id, string RefNumber, CancellationToken cancellationToken);
        Task<int> PrescriptionStatusCancelled(CancellationToken cancellationToken);
    }
}
