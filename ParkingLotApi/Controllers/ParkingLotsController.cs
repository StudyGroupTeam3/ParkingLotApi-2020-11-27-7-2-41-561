using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ParkingLotsController : ControllerBase
    {
        private readonly ParkingLotService parkingLotService;

        public ParkingLotsController(ParkingLotService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLotDto>> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            var name = await this.parkingLotService.AddParkingLot(parkingLotDto);
            if (name != null)
            {
                return CreatedAtAction(nameof(GetParkingLotByName), new { name = name }, parkingLotDto);
            }

            return BadRequest("please valid your parking lot information again");
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteParkingLot(string parkingLotName)
        {
            return this.NoContent();
        }

        [HttpGet("/pages/{index}")]
        public async Task<ActionResult<List<ParkingLotDto>>> GetParkingLotByPageIndex(int pageIndex)
        {
            return null;
        }

        [HttpGet("/{name}")]
        public async Task<ActionResult<ParkingLotDto>> GetParkingLotByName(string parkingLotName)
        {
            return null;
        }

        [HttpPatch("/{name}")]
        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotCapacity(string parkingLotName)
        {
            return null;
        }
    }
}
