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
        private readonly ParkingLotService parkingLotService;

        public ParkingOrdersController(ParkingOrderService parkingOrderService, ParkingLotService parkingLotService)
        {
            this.parkingOrderService = parkingOrderService;
            this.parkingLotService = parkingLotService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingOrderDto>> AddParkingOrder(ParkingOrderDto parkingOrderDto)
        {
            var orderFound = parkingOrderService.GetAllParkingOrders().Result
                .FirstOrDefault(parkingOrderEntity => parkingOrderEntity.PlateNumber == parkingOrderDto.PlateNumber);
            if (orderFound != null)
            {
                return BadRequest("car in the lot");
            }

            var parkingLot = await parkingLotService.GetParkingLotByName(parkingOrderDto.ParkingLotName);
            var capacity = parkingLot.Capacity;
            var occupies = await parkingLotService.GetParkingLotCapacityByName(parkingOrderDto.ParkingLotName);
            if (occupies == -1)
            {
                return BadRequest("parking lot not found");
            }

            if (capacity == occupies)
            {
                return BadRequest("parking lot is full");
            }

            var parkingOrderNumber = await parkingOrderService.AddParkingOrder(parkingOrderDto);

            return CreatedAtAction(nameof(GetParkingOrderByOrderNumber), new { orderNumber = parkingOrderNumber }, parkingOrderNumber);
        }

        [HttpGet("{orderNumber}")]
        public async Task<ActionResult<ParkingOrderDto>> GetParkingOrderByOrderNumber(string orderNumber)
        {
            var foundParkingOrder = await parkingOrderService.GetParkingOrderByOrderNumber(orderNumber);
            if (foundParkingOrder != null)
            {
                return Ok(foundParkingOrder);
            }

            return NotFound();
        }

        [HttpPatch("{parkingOrderNumber}")]
        public async Task<ActionResult<ParkingOrderDto>> UpdateParkingLotCapacity(string parkingOrderNumber, UpdateParkingOrderDto updateParkingOrderDto)
        {
            var foundParkingOrder = await parkingOrderService.GetParkingOrderByOrderNumber(parkingOrderNumber);
            if (foundParkingOrder != null)
            {
                var changedParkingOrder = await parkingOrderService.UpdateParkingOrder(parkingOrderNumber, updateParkingOrderDto);
                return Ok(changedParkingOrder);
            }

            return NotFound("parking order not found");
        }
    }
}
