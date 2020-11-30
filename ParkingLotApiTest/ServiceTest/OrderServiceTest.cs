using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System.Threading.Tasks;
using ParkingLotApi.Models;
using Xunit;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("ParkingLotTest")]
    public class OrderServiceTest : TestBase
    {
        private readonly ParkingLotContext context;
        private readonly ParkingLotService parkingLotService;
        private readonly OrderService orderService;
        public OrderServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            context = scopeService.GetRequiredService<ParkingLotContext>();
            parkingLotService = new ParkingLotService(context);
            orderService = new OrderService(context);
        }

        [Fact]
        public async Task Story2_AC1_3_Should_add_order_correctly()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");
            var order = new OrderRequest("Lot1", "JA00001");

            // when
            await parkingLotService.AddParkingLot(parkingLot);
            var orderEntity = await orderService.AddOrder(order);

            // then
            Assert.Equal(orderEntity, context.Orders.FirstOrDefaultAsync().Result);
            Assert.Equal(Status.Open, context.Orders.FirstOrDefaultAsync().Result.Status);
        }

        [Fact]
        public async Task Story1_AC5_Should_update_parkingLot_capacity()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");
            var closeTime = DateTime.Now;
            var updateModel = new OrderUpdateModel(closeTime, Status.Close);
            var order = new OrderRequest("Lot1", "JA00001");

            // when
            await parkingLotService.AddParkingLot(parkingLot);
            await orderService.AddOrder(order);
            await orderService.UpdateOrder(context.Orders.FirstOrDefaultAsync().Result.OrderNumber, updateModel);

            // then
            Assert.Equal(updateModel.Status, context.Orders.FirstOrDefaultAsync().Result.Status);
            Assert.Equal(closeTime, context.Orders.FirstOrDefaultAsync().Result.CloseTime);
        }
    }
}
