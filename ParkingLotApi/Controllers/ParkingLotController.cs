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
        public async Task<ActionResult<ParkingLot>> AddParkingLot(ParkingLot parkingLot)
        {
            var id = await service.AddParkingLot(parkingLot);

            return CreatedAtAction(nameof(GetParkingLotById), new { id = id }, parkingLot);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLot>> GetParkingLotById(int id)
        {
            var lot = await service.GetParkingLotById(id);

            return lot == null ? (ActionResult<ParkingLot>)NotFound("no lot match id") : Ok(lot);
        }
    }
}
