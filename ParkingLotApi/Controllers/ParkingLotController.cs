using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Models;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkingLots")]
    public class ParkingLotController : ControllerBase
    {
        private readonly ParkingLotService service;

        public ParkingLotController(ParkingLotService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLot>> Add(ParkingLot parkingLot)
        {
            var lotFound = service.GetAllParkingLots().Result.FirstOrDefault(lot => lot.Name == parkingLot.Name);

            if (lotFound != null)
            {
                return BadRequest("lot with same name exists");
            }

            var id = await service.AddParkingLot(parkingLot);

            return CreatedAtAction(nameof(GetById), new { id = id }, parkingLot);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLot>> GetById(int id)
        {
            var lotFound = await service.GetParkingLotById(id);

            return lotFound == null ? (ActionResult<ParkingLot>)NotFound("no lot match id") : Ok(lotFound);
        }

        [HttpGet]
        public async Task<ActionResult<List<ParkingLot>>> GetAll()
        {
            var lots = await service.GetAllParkingLots();

            return Ok(lots);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lotFound = await service.GetParkingLotById(id);

            if (lotFound == null)
            {
                return NotFound("no lot match id");
            }

            await service.DeleteParkingLot(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, ParkingLotUpdateModel data)
        {
            var lotFound = await service.GetParkingLotById(id);

            if (lotFound == null)
            {
                return NotFound("no lot match id");
            }

            await service.UpdateParkingLot(id, data);
            return NoContent();
        }
    }
}
