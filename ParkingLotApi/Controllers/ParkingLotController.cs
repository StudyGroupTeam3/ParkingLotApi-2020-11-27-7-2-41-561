using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Entities;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkingLots")]
    public class ParkingLotController : ControllerBase
    {
        private readonly ParkingLotService service;
        private readonly OrderService orderService;

        public ParkingLotController(ParkingLotService service, OrderService orderService)
        {
            this.service = service;
            this.orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLot>> Add(ParkingLot parkingLot)
        {
            if (parkingLot.Location == null || parkingLot.Name == null)
            {
                return BadRequest("location or name can not be empty");
            }

            if (parkingLot.Capacity < 0)
            {
                return BadRequest("capacity can not be negative");
            }

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

            var orderOpen = orderService.GetAllOrderEntities().Result
                .FirstOrDefault(order => order.ParkingLotName == name && order.Status == Status.Open);

            if (orderOpen != null)
            {
                return BadRequest("some car still in the lot");
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

            if (data.Capacity < 0)
            {
                return BadRequest("capacity can not be negative");
            }

            await service.UpdateParkingLot(name, data);

            return NoContent();
        }
    }
}
