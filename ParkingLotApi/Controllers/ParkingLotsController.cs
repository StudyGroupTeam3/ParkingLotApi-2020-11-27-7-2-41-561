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
                return CreatedAtAction(nameof(GetParkingLotByName), new { parkingLotName = name }, parkingLotDto);
            }

            return BadRequest("please valid your parking lot information again");
        }

        [HttpDelete("{parkingLotName}")]
        public async Task<ActionResult> DeleteParkingLot(string parkingLotName)
        {
            await this.parkingLotService.DeleteParkingLot(parkingLotName); 
            return NoContent();
        }

        [HttpGet("pages/{pageIndex}")]
        public async Task<ActionResult<List<ParkingLotDto>>> GetParkingLotByPageIndex(int pageIndex)
        {
            var allLots = await parkingLotService.GetParkingLotsByPage(pageIndex);
            return Ok(allLots);
        }

        [HttpGet("{parkingLotName}")]
        public async Task<ActionResult<ParkingLotDto>> GetParkingLotByName(string parkingLotName)
        {
            var parkingLotDto = await this.parkingLotService.GetParkingLotByName(parkingLotName);
            return Ok(parkingLotDto);
        }

        [HttpPatch("{parkingLotName}")]
        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotCapacity(string parkingLotName, UpdateParkingLotCapacityDto updateParkingLotCapacity)
        { 
            var changedParkingLotDto = await parkingLotService.UpdateParkingLotCapacity(parkingLotName, updateParkingLotCapacity);
            return Ok(changedParkingLotDto);
        }
    }
}
