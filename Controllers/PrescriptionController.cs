using MedicineSystemAPI.CustomModels;
using MedicineSystemAPI.Interfaces;
using MedicineSystemAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionRepository _repository;

        public PrescriptionController(IPrescriptionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<TblPrescription_OrderOutput>> AddPrescription([FromBody] TblPrescription_OrderInput input, CancellationToken cancellationToken)
        {
            var datas = await _repository.AddPrescription(input, cancellationToken);
            return Ok(datas);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<TblPrescription_OrderOutput>> GetPrescription(string refNumber, string Name, CancellationToken cancellationToken)
        {
            var datas = await _repository.GetPrescription(refNumber, Name, cancellationToken);
            return Ok(datas);
        }

        [Authorize]
        [HttpGet]
        //[AllowAnonymous]
        [Route("status")]
        public async Task<ActionResult<List<TblStatus>>> GetPrescriptionStatus( CancellationToken cancellationToken)
        {
            var datas = await _repository.GetPrescriptionStatus(cancellationToken);
            return Ok(datas);
        }

        [HttpPut]
        [Route("status/reserved/{Id}")]
        public async Task<ActionResult<bool>> PrescriptionStatusReserved(int Id, [FromBody] RefNumber_Input input, CancellationToken cancellationToken)
        {
            var datas = await _repository.PrescriptionStatusReserved(Id, input.RefNumber, cancellationToken);
            return Ok(datas);
        }

        [HttpPut]
        [Route("status/progress/{Id}")]
        public async Task<ActionResult<bool>> PrescriptionStatusInProgress(int Id, [FromBody] RefNumber_Input input, CancellationToken cancellationToken)
        {
            var datas = await _repository.PrescriptionStatusInProgress(Id, input.RefNumber, cancellationToken);
            return Ok(datas);
        }

        [HttpPut]
        [Route("status/finish/{Id}")]
        public async Task<ActionResult<bool>> PrescriptionStatusFinish(int Id, [FromBody] RefNumber_Input input, CancellationToken cancellationToken)
        {
            var datas = await _repository.PrescriptionStatusFinished(Id, input.RefNumber, cancellationToken);
            return Ok(datas);
        }

        [HttpPut]
        [Route("status/complete/{Id}")]
        public async Task<ActionResult<bool>> PrescriptionStatusComplete(int Id, [FromBody] RefNumber_Input input, CancellationToken cancellationToken)
        {
            var datas = await _repository.PrescriptionStatusCompleted(Id, input.RefNumber, cancellationToken);
            return Ok(datas);
        }

        [HttpPut]
        [Route("status/cancel")]
        public async Task<ActionResult<int>> PrescriptionStatusCancel(CancellationToken cancellationToken)
        {
            var datas = await _repository.PrescriptionStatusCancelled(cancellationToken);
            return Ok(datas);
        }
    }
}
