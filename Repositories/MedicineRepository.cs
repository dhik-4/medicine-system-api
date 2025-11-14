using MedicineSystemAPI.Interfaces;
using MedicineSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicineSystemAPI.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        AppDbContext _context;

        public MedicineRepository(AppDbContext dbContext)
        {
            //_custContext = custContext;
            _context = dbContext;
        }

        public async Task<List<TblMedicine>> GetMedicine( CancellationToken cancellationToken)
        {
            List<TblMedicine> Result = null;
            try
            {
                var getTbl = _context.TblMedicines.Where(p => 1 == 1);

                var _result = await getTbl.ToListAsync(cancellationToken);
                Result = _result;
            }
            catch (Exception ex)
            {
            }

            return Result;
        }

        public async Task<bool> AddMedicine(TblMedicine data, CancellationToken cancellationToken)
        {
            bool Result = false;
            try
            {
                _context.TblMedicines.Add(data);
                await _context.SaveChangesAsync(cancellationToken);

                _context.ChangeTracker.Clear();

                Result = true;
            }
            catch (Exception ex)
            {
            }

            return Result;
        }

        public async Task<bool> UpdateMedicine(TblMedicine data, CancellationToken cancellationToken)
        {
            bool Result = false;
            try
            {
                _context.TblMedicines.Where(t => t.MedicineId == data.MedicineId)
                    .ExecuteUpdateAsync(s =>
                        s.SetProperty(v => v.Name, data.Name)
                        .SetProperty(v => v.Price, data.Price)
                        .SetProperty(v => v.Stock, data.Stock)
                        .SetProperty(v => v.Code, data.Code)
                        .SetProperty(v => v.Note, data.Note)
                    );

                await _context.SaveChangesAsync(cancellationToken);

                _context.ChangeTracker.Clear();

                Result = true;
            }
            catch (Exception ex)
            {
            }

            return Result;
        }
    }
}
