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
    public class ParkController : ControllerBase
    {
        private ParkService parkService;

        public ParkController(ParkService parkService)
        {
            this.parkService = parkService;
        }

        [HttpPost("parking")]
        public async Task<ActionResult<ParkingOrderDto>> ParkCar(ParkingOrderDto parkingOrderDto)
        {
            if (!parkService.IsFreeSpaceInParkingLot(parkingOrderDto.NameOfParkingLot))
            {
                return BadRequest("The parking lot is full");
            }

            var actualParkingOrderDto = await parkService.ParkCar(parkingOrderDto);
            return Ok(actualParkingOrderDto);
        }

        [HttpPatch("parking")]
        public async Task<ActionResult> Leave(ParkingOrderDto parkingOrderDto)
        {
            await parkService.Leave(parkingOrderDto);
            return Ok();
        }
    }
}
