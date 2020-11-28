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
            if (parkinglotDto.Name == null || parkinglotDto.Capacity <= 0 || parkinglotDto.Location == null
            || parkinglotDto.Name == string.Empty || parkinglotDto.Location == string.Empty)
            {
                return BadRequest(new
                {
                    message = $"Should input all information and input positive capacity."
                });
            }

            var id = await parkingLotService.AddParkingLotAsnyc(parkinglotDto);
            if (id == -1)
            {
                return Conflict(new
                {
                    message = $"An existing parkinglot " +
                                                $"with the name '{parkinglotDto.Name}' was already found."
                });
            }

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
