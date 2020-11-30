﻿using System;
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

            int parkingLotId = await this.parkingLotService.AddParkingLot(parkingLotDto);
            return CreatedAtAction(nameof(GetParkingLotById), new { parkingLotId = parkingLotId }, parkingLotDto);
        }

        [HttpGet("parkinglots")]
        public async Task<ActionResult<int>> GetParkingLotsByPage(int pageSize = 15, int pageIndex = 1)
        {
            return Ok(await this.parkingLotService.GetParkingLotsByPage(pageSize, pageIndex));
        }

        [HttpGet("parkinglots/{parkingLotId:int}")]
        public async Task<ActionResult<ParkingLotDto>> GetParkingLotById(int parkingLotId)
        {
            var parkingLotDto = await this.parkingLotService.GetParkingLotById(parkingLotId);
            if (parkingLotDto is null)
            {
                return NotFound();
            }

            return Ok(parkingLotDto);
        }

        [HttpDelete("parkinglots/{parkingLotId:int}")]
        public async Task<ActionResult<ParkingLotDto>> DeleteParkingLotById(int parkingLotId)
        {
            var parkingLotDto = await this.parkingLotService.DeleteParkingLotById(parkingLotId);
            if (parkingLotDto is null)
            {
                return NotFound();
            }

            return Ok(parkingLotDto);
        }

        [HttpPatch("parkinglots/{parkingLotId:int}")]
        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotCapacityById(int parkingLotId, ParkingLotCapacityUpdateDto parkingLotCapacityUpdateModel)
        {
            var parkingLotDto = await this.parkingLotService.UpdateParkingLotCapacityById(parkingLotId, parkingLotCapacityUpdateModel);
            if (parkingLotDto is null)
            {
                return NotFound();
            }

            return Ok(parkingLotDto);
        }
    }
}
