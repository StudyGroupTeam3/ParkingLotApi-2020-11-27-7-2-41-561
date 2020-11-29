using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("Orders")]
    public class ParkingOrderController : ControllerBase
    {
        private readonly ParkingOrderService parkingOrderService;
        // private readonly List<ParkingOrderDto> parkingOrderDtos = new ParkingOrderDto();

        public ParkingOrderController(ParkingOrderService parkingOrderService)
        {
            this.parkingOrderService = parkingOrderService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingOrderDto>> AddOrder(ParkingOrderDto parkingOrderDto)
        {
            var id = await this.parkingOrderService.AddParkingOrder(parkingOrderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = id }, parkingOrderDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetOrderById(int id)
        {
            var parkingLotDto = await this.parkingOrderService.GetOrderById(id);
            return Ok(parkingLotDto);
        }
    }
}
