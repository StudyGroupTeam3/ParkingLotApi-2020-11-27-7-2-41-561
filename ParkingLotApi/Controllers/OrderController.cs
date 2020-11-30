using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Models;
using ParkingLotApi.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly ParkingLotService parkingLotService;

        public OrderController(OrderService orderService, ParkingLotService parkingLotService)
        {
            this.orderService = orderService;
            this.parkingLotService = parkingLotService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Add(OrderRequest order)
        {
            var lotFound = await parkingLotService.GetParkingLotByName(order.ParkingLotName);
            if (lotFound == null)
            {
                return BadRequest("no parkingLot match the name");
            }

            var orderFound = orderService.GetOrders().Result
                .FirstOrDefault(orderEntity => orderEntity.PlateNumber == order.PlateNumber
                                               && orderEntity.Status == Status.Open);
            if (orderFound != null)
            {
                return BadRequest("car in the lot");
            }

            var emptyPosition = await parkingLotService.GetParkingLotEmptyPositionByName(order.ParkingLotName);
            if (emptyPosition == 0)
            {
                return BadRequest("The parking lot is full");
            }

            var orderReturn = await orderService.AddOrder(order);

            return CreatedAtAction(nameof(GetByNumber), new { number = orderReturn.OrderNumber }, orderReturn);
        }

        [HttpGet("{number}")]
        public async Task<ActionResult<Order>> GetByNumber(int number)
        {
            var orderFound = await orderService.GetOrderByNumber(number);

            return orderFound == null ? (ActionResult<Order>)NotFound("unrecognized order number") : Ok(orderFound);
        }

        [HttpPatch("{number}")]
        public async Task<IActionResult> Patch(int number, OrderUpdateModel data)
        {
            var orderFound = await orderService.GetOrderByNumber(number);

            if (orderFound == null)
            {
                return NotFound("unrecognized order number");
            }

            var orderEntity = await orderService.GetOrderEntityByNumber(number);
            if (orderEntity.Status == Status.Close)
            {
                return BadRequest("car has leaved");
            }

            await orderService.UpdateOrder(number, data);

            return NoContent();
        }
    }
}
