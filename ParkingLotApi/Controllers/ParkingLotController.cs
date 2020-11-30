using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class ParkingLotController : ControllerBase
    {
        private ParkingLotService parkingLotService;

        public ParkingLotController(ParkingLotService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpPost("parkinglots")]
        public async Task<ActionResult<int>> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            if (parkingLotService.IsParkingLotNameExisting(parkingLotDto.Name))
            {
                return BadRequest("Parking lot name already exists!");
            }

            string parkingLotName = await this.parkingLotService.AddParkingLot(parkingLotDto);
            return CreatedAtAction(nameof(GetParkingLotByName), new { parkingLotName = parkingLotName }, parkingLotDto);
        }

        [HttpGet("parkinglots")]
        public async Task<ActionResult<int>> GetParkingLotsByPage(int pageSize = 15, int pageIndex = 1)
        {
            return Ok(await this.parkingLotService.GetParkingLotsByPage(pageSize, pageIndex));
        }

        [HttpGet("parkinglots/{parkingLotName}")]
        public async Task<ActionResult<ParkingLotDto>> GetParkingLotByName(string parkingLotName)
        {
            var parkingLotDto = await this.parkingLotService.GetParkingLotByName(parkingLotName);
            if (parkingLotDto is null)
            {
                return NotFound();
            }

            return Ok(parkingLotDto);
        }

        [HttpDelete("parkinglots/{parkingLotName}")]
        public async Task<ActionResult<ParkingLotDto>> DeleteParkingLotByName(string parkingLotName)
        {
            var parkingLotDto = await this.parkingLotService.DeleteParkingLotByName(parkingLotName);
            if (parkingLotDto is null)
            {
                return NotFound();
            }

            return Ok(parkingLotDto);
        }

        [HttpPatch("parkinglots/{parkingLotName}")]
        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotCapacityByName(string parkingLotName, ParkingLotCapacityUpdateDto parkingLotCapacityUpdateModel)
        {
            var parkingLotDto = await this.parkingLotService.UpdateParkingLotCapacityByName(parkingLotName, parkingLotCapacityUpdateModel);
            if (parkingLotDto is null)
            {
                return NotFound();
            }

            return Ok(parkingLotDto);
        }
    }
}
