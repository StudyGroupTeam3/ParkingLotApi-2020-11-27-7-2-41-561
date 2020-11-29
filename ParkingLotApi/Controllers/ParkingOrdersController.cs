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
    public class ParkingOrdersController : ControllerBase
    {
        private readonly ParkingOrderService parkingOrderService;

        public ParkingOrdersController(ParkingOrderService parkingOrderService)
        {
            this.parkingOrderService = parkingOrderService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingOrderDto>> AddParkingOrder(ParkingOrderDto parkingOrderDto)
        {
            return BadRequest("please valid your parking lot information again");
        }

        [HttpPatch("{parkingOrderNumber}")]
        public async Task<ActionResult<ParkingOrderDto>> UpdateParkingLotCapacity(string parkingOrderNumber, UpdateParkingOrderDto updateParkingOrderDto)
        {
            return Ok();
        }
    }
}
