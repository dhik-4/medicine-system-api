using MedicineSystemAPI.CustomModels;
using MedicineSystemAPI.Interfaces;
using MedicineSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicineSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _repository;

        public DoctorController(IDoctorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<TblDoctor>>> GetDoctorList(string? doctorName, string? SIP, CancellationToken cancellationToken)
        {
            var datas = await _repository.GetDoctors(doctorName, SIP, cancellationToken);
            return Ok(datas);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddDoctor([FromBody] TblDoctor_Input input, CancellationToken cancellationToken)
        {
            TblDoctor data = new TblDoctor()
            {
                Name = input.Name,
                CreatedDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Sip = input.Sip,
                Speciality = input.Speciality,
                Note = input.Note,
            };

            var datas = await _repository.AddDoctors(data, cancellationToken);
            return Ok(datas);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateDoctor(int Id, [FromBody] TblDoctor_Input input, CancellationToken cancellationToken)
        {
            TblDoctor data = new TblDoctor()
            {
                DoctorId = Id,
                Name = input.Name,
                //CreatedDate = DateTime.Now,
                //UpdateDate = DateTime.Now,
                Sip = input.Sip,
                Speciality = input.Speciality,
                Note = input.Note,
            };

            var datas = await _repository.UpdateDoctors(data, cancellationToken);
            return Ok(datas);
        }
    }
}
