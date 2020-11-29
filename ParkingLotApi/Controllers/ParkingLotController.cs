using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Services;
using System.Collections.Generic;
using System.Linq;
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
            var lotFound = service.GetAllParkingLots().Result.FirstOrDefault(lot => lot.Name == parkingLot.Name);

            if (lotFound != null)
            {
                return BadRequest("lot with same name exists");
            }

            var name = await service.AddParkingLot(parkingLot);

            return CreatedAtAction(nameof(GetByName), new { name = name }, parkingLot);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ParkingLot>> GetByName(string name)
        {
            var lotFound = await service.GetParkingLotByName(name);

            return lotFound == null ? (ActionResult<ParkingLot>)NotFound("no lot match name") : Ok(lotFound);
        }

        [HttpGet]
        public async Task<ActionResult<List<ParkingLot>>> GetAll(int page)
        {
            if (page != 0)
            {
                var lots = await service.GetAllParkingLots(page);
                return Ok(lots);
            }

            var allLots = await service.GetAllParkingLots();
            return Ok(allLots);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var lotFound = await service.GetParkingLotByName(name);

            if (lotFound == null)
            {
                return NotFound("no lot match name");
            }

            await service.DeleteParkingLot(name);
            return NoContent();
        }

        [HttpPatch("{name}")]
        public async Task<IActionResult> Patch(string name, ParkingLotUpdateModel data)
        {
            var lotFound = await service.GetParkingLotByName(name);

            if (lotFound == null)
            {
                return NotFound("no lot match name");
            }

            await service.UpdateParkingLot(name, data);
            return NoContent();
        }
    }
}
