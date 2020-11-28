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
            int parkingLotId = await this.parkingLotService.AddParkingLot(parkingLotDto);
            return Ok(parkingLotId);
        }
    }
}
