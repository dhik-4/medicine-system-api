using MedicineSystemAPI.CustomModels;
using MedicineSystemAPI.Interfaces;
using MedicineSystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedicineSystemAPI.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        AppDbContext _context;

        public DoctorRepository( AppDbContext dbContext)
        {
            //_custContext = custContext;
            _context = dbContext;
        }

        public async Task<List<TblDoctor>> GetDoctors(string doctorName, string SIP, CancellationToken cancellationToken)
        {
            var getTbl = _context.TblDoctors.Where(p => 1 == 1);
            if (!string.IsNullOrWhiteSpace(doctorName))
            {
                getTbl = getTbl.Where(d => d.Name == doctorName);
            }
            if(!string.IsNullOrWhiteSpace(SIP))
            {
                getTbl = getTbl.Where(d => d.Sip == SIP);
            }
            var _result = await getTbl.ToListAsync(cancellationToken);

            return _result;
        }

        public async Task<bool> AddDoctors(TblDoctor data, CancellationToken cancellationToken)
        {
            bool Result = false;
            try
            {
                _context.TblDoctors.Add(data);
                await _context.SaveChangesAsync(cancellationToken);

                _context.ChangeTracker.Clear();

                Result = true;
            }
            catch (Exception ex)
            {
            }

            return Result;
        }

        public async Task<bool> UpdateDoctors(TblDoctor data, CancellationToken cancellationToken)
        {
            bool Result = false;
            try
            {
                _context.TblDoctors.Where(t => t.DoctorId == data.DoctorId)
                    .ExecuteUpdateAsync(s =>
                        s.SetProperty(v => v.Name, data.Name)
                        .SetProperty(v => v.Speciality, data.Speciality)
                        .SetProperty(v => v.Sip, data.Sip)
                        .SetProperty(v => v.Note, data.Note)
                        .SetProperty(v => v.UpdateDate, DateTime.Now)
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
