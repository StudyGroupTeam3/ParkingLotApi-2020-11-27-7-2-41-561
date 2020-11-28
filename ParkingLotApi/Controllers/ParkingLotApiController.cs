using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingLotApi.DTO;
using ParkingLotApi.Service;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingLotApiController : ControllerBase
    {
        private readonly ParkingLotApiService parkingLotService;
        public ParkingLotApiController(ParkingLotApiService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpPost("ParkingLots")]
        public async Task<ActionResult<ParkinglotDTO>> CreateParkingLot(ParkinglotDTO parkinglotDto)
        {
            var id = await parkingLotService.AddParkingLotAsnyc(parkinglotDto);
            return CreatedAtAction(nameof(GetById), new { id = id }, parkinglotDto);
        }

        [HttpGet("ParkingLots/{id}")]
        public async Task<ActionResult<ParkinglotDTO>> GetById(int id)
        {
            var parkinglotDto = await this.parkingLotService.GetById(id);
            return Ok(parkinglotDto);
        }
    }
}
