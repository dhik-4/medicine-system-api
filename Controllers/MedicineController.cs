using MedicineSystemAPI.CustomModels;
using MedicineSystemAPI.Interfaces;
using MedicineSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedicineSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineRepository _repository;

        public MedicineController(IMedicineRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<TblMedicine>>> GetMedicineList( CancellationToken cancellationToken)
        {
            var datas = await _repository.GetMedicine( cancellationToken);
            return Ok(datas);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddMedicine([FromBody] TblMedicine_Input input, CancellationToken cancellationToken)
        {
            TblMedicine data = new TblMedicine()
            {
                Name = input.Name,
                Code = input.Code,
                Price = input.Price,
                Stock = input.Stock,
                CreatedDate = DateTime.Now,
                Note = input.Note,
            };

            var datas = await _repository.AddMedicine(data, cancellationToken);
            return Ok(datas);
        }
    }
}
