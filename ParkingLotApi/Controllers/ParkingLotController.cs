using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Threading.Tasks;

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
            var id = await service.AddParkingLot(parkingLot);

            return CreatedAtAction(nameof(GetById), new { id = id }, parkingLot);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLot>> GetById(int id)
        {
            var lot = await service.GetParkingLotById(id);

            return lot == null ? (ActionResult<ParkingLot>)NotFound("no lot match id") : Ok(lot);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lot = await service.GetParkingLotById(id);

            if (lot == null)
            {
                return NotFound("no lot match id");
            }

            await service.DeleteParkingLot(id);
            return NoContent();
        }
    }
}
