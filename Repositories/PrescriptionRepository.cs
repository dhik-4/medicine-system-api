using MedicineSystemAPI.CustomModels;
using MedicineSystemAPI.Interfaces;
using MedicineSystemAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedicineSystemAPI.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PrescriptionRepository(AppDbContext dbContext, IConfiguration configuration)
        {
            //_custContext = custContext;
            _context = dbContext;
            _configuration = configuration;
        }

        public async Task<TblPrescription_OrderOutput> AddPrescription(TblPrescription_OrderInput input, CancellationToken cancellationToken)
        {
            TblPrescription_OrderOutput Result = new TblPrescription_OrderOutput();
            try
            {
                bool isMedicineAvailable = true;
                decimal? totalPrice = 0;

                var dataMedicines = input.Medicines;
                for (int i = 0; i < dataMedicines.Count; i++)
                {
                    var getMedStock = await _context.TblMedicines.Where(p => p.MedicineId == dataMedicines[i].MedicineID).FirstOrDefaultAsync(cancellationToken);
                    if (getMedStock != null && getMedStock.Stock >= dataMedicines[i].Qty)
                    {
                        _context.TblMedicines.Where(t => t.MedicineId == dataMedicines[i].MedicineID)
                            .ExecuteUpdateAsync(s =>
                                s.SetProperty(v => v.Stock, (getMedStock.Stock - dataMedicines[i].Qty))
                            );
                        await _context.SaveChangesAsync(cancellationToken);

                        totalPrice += getMedStock.Price * dataMedicines[i].Qty;
                    }
                    else
                    {
                        isMedicineAvailable = false;
                        break;
                    }
                }

                if (isMedicineAvailable)
                {
                    string jsonMedicine = JsonSerializer.Serialize(dataMedicines);
                    TblPrescription data = new TblPrescription()
                    {
                        RefNumber = "PRE" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        PatientName = input.PatientName,
                        Medicines=jsonMedicine,
                        Description = input.Description,
                        DoctorsId = input.DoctorsId,

                        Note = input.Note,
                        StatusId = 1,
                        TotalPrice = totalPrice
                    };

                    _context.TblPrescriptions.Add(data);
                    await _context.SaveChangesAsync(cancellationToken);

                    var _doctor = await _context.TblDoctors.Where(p => p.DoctorId == data.DoctorsId).FirstOrDefaultAsync(cancellationToken);
                    var _status = await _context.TblStatuses.Where(p => p.StatusId == data.StatusId).FirstOrDefaultAsync(cancellationToken);

                    Result.RefNumber = data.RefNumber;
                    Result.PatientName = input.PatientName;
                    Result.Doctor = (_doctor).Name;
                    Result.Description = input.Description;
                    Result.TotalPrice = totalPrice;
                    Result.Status = _status.Description;
                    Result.Note = input.Note;
                }
                _context.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
            }

            return Result;
        }

        public async Task<TblPrescription_OrderOutput> GetPrescription(string refNumber, string Name, CancellationToken cancellationToken)
        {
            TblPrescription_OrderOutput Result = new TblPrescription_OrderOutput();
            try
            {
                var getPrescription = await _context.TblPrescriptions.Where(p => p.RefNumber == refNumber).Where(p => p.PatientName == Name)
                .FirstOrDefaultAsync(cancellationToken);

                if (getPrescription != null)
                {
                    List<TblMedicine_Order> _medicines = JsonSerializer.Deserialize<List<TblMedicine_Order>>(getPrescription.Medicines);
                    var _medicinesOut = new List<TblMedicine_OrderOut>();
                    for (int i = 0; i < _medicines.Count; i++)
                    {
                        var _medicinesName = (await _context.TblMedicines.Where(p => p.MedicineId == _medicines[i].MedicineID)
                            .FirstOrDefaultAsync(cancellationToken)).Name;

                        _medicinesOut.Add(new TblMedicine_OrderOut 
                        {
                            Medicine = _medicinesName,
                            Qty = _medicines[i].Qty 
                        });
                    }

                    var _doctor = await _context.TblDoctors.Where(p => p.DoctorId == getPrescription.DoctorsId).FirstOrDefaultAsync(cancellationToken);
                    var _status = await _context.TblStatuses.Where(p => p.StatusId == getPrescription.StatusId).FirstOrDefaultAsync(cancellationToken);

                    Result = new TblPrescription_OrderOutput()
                    {
                        RefNumber = refNumber,
                        PatientName = Name,
                        Medicines = _medicinesOut,
                        Description = getPrescription.Description,
                        Doctor = _doctor.Name,
                        Note = getPrescription.Note,
                        Status = _status.Description,
                        TotalPrice = getPrescription.TotalPrice
                    };
                }
            }
            catch (Exception ex)
            {
            }

            return Result;
        }

        public async Task<List<TblStatus>> GetPrescriptionStatus(CancellationToken cancellationToken)
        {
            List<TblStatus> _result = new List<TblStatus>();
            try
            {
                _result = await _context.TblStatuses.ToListAsync(cancellationToken);
            }
            catch
            {
            }

            return _result;
        }

        public async Task<bool> PrescriptionStatusReserved(long Id, string RefNumber, CancellationToken cancellationToken)
        {
            bool _result = false;
            int rowsAffected= 0;
            byte newStatusId = 2;
            try
            {
                var _pres = await _context.TblPrescriptions.Where(p => p.PrescriptionId == Id).FirstOrDefaultAsync(cancellationToken);
                if (_pres != null && _pres.RefNumber == RefNumber && _pres.StatusId == 1)
                {
                    rowsAffected = await _context.TblPrescriptions.Where(t => t.PrescriptionId == Id)
                        .ExecuteUpdateAsync(s =>
                            s.SetProperty(v => v.StatusId, newStatusId)
                        );

                    //rowsAffected = await _context.SaveChangesAsync(cancellationToken);

                    _context.ChangeTracker.Clear();

                    _result = true;
                }
            }
            catch
            {
            }

            return _result;
        }

        public async Task<bool> PrescriptionStatusInProgress(long Id, string RefNumber, CancellationToken cancellationToken)
        {
            bool _result = false;
            byte newStatusId = 3;
            try
            {
                var _pres = await _context.TblPrescriptions.Where(p => p.PrescriptionId == Id).FirstOrDefaultAsync(cancellationToken);
                if (_pres != null && _pres.RefNumber == RefNumber && _pres.StatusId == 2)
                {
                    await _context.TblPrescriptions.Where(t => t.PrescriptionId == Id)
                        .ExecuteUpdateAsync(s =>
                            s.SetProperty(v => v.StatusId, newStatusId)
                        );

                    //await _context.SaveChangesAsync(cancellationToken);

                    _context.ChangeTracker.Clear();

                    _result = true;
                }
            }
            catch
            {
            }

            return _result;
        }

        public async Task<bool> PrescriptionStatusFinished(long Id, string RefNumber, CancellationToken cancellationToken)
        {
            bool _result = false;
            byte newStatusId = 4;
            try
            {
                var _pres = await _context.TblPrescriptions.Where(p => p.PrescriptionId == Id).FirstOrDefaultAsync(cancellationToken);
                if (_pres != null && _pres.RefNumber == RefNumber && _pres.StatusId == 3)
                {
                    await _context.TblPrescriptions.Where(t => t.PrescriptionId == Id)
                        .ExecuteUpdateAsync(s =>
                            s.SetProperty(v => v.StatusId, newStatusId)
                        );

                    //await _context.SaveChangesAsync(cancellationToken);

                    _context.ChangeTracker.Clear();

                    _result = true;
                }
            }
            catch
            {
            }

            return _result;
        }

        public async Task<bool> PrescriptionStatusCompleted(long Id, string RefNumber, CancellationToken cancellationToken)
        {
            bool _result = false;
            byte newStatusId = 5;
            try
            {
                var _pres = await _context.TblPrescriptions.Where(p => p.PrescriptionId == Id).FirstOrDefaultAsync(cancellationToken);
                if (_pres != null && _pres.RefNumber == RefNumber && _pres.StatusId == 4)
                {
                    await _context.TblPrescriptions.Where(t => t.PrescriptionId == Id)
                        .ExecuteUpdateAsync(s =>
                            s.SetProperty(v => v.StatusId, newStatusId)
                        );

                    //await _context.SaveChangesAsync(cancellationToken);

                    _context.ChangeTracker.Clear();

                    _result = true;
                }
            }
            catch
            {
            }

            return _result;
        }

        public async Task<int> PrescriptionStatusCancelled(CancellationToken cancellationToken)
        {
            int rowsAffected = 0;
            int DayToCancel = _configuration.GetValue<int>("CustomVariable:MaxCancelDay");
            byte newStatusId = 6;
            try
            {
                var _pres = await _context.TblPrescriptions.Where(p => p.StatusId == 1 && p.UpdatedDate < DateTime.Now.AddDays(- DayToCancel))
                    .ToListAsync(cancellationToken);
                if (_pres != null && _pres.Count > 0)
                {
                    rowsAffected = await _context.TblPrescriptions.Where(t => t.StatusId == 1 && t.UpdatedDate < DateTime.Now.AddDays(- DayToCancel))
                        .ExecuteUpdateAsync(s =>
                            s.SetProperty(v => v.StatusId, newStatusId)
                        );

                    _context.ChangeTracker.Clear();
                }
            }
            catch
            {
            }

            return rowsAffected;
        }
    }
}
