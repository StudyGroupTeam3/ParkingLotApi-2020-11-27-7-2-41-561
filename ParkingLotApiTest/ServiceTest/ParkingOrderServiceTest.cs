using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingOrderTest")]
    public class ParkingOrderServiceTest : TestBase
    {
        public ParkingOrderServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_parkingOrder_when_add_parkingOrder_via_parkingLotService()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            ParkingLotContext context = scopedServices.GetRequiredService<ParkingLotContext>();
            var parkingOrderDto = GenerateParkingOrderDto();

            ParkingOrderService parkingOrderService = new ParkingOrderService(context);
            var parkingOrderNumber = await parkingOrderService.AddParkingOrder(parkingOrderDto);
            var foundParkingOrder = await context.ParkingOrders.FirstOrDefaultAsync(parkingOrderEntity => parkingOrderEntity.OrderNumber == parkingOrderNumber);

            Assert.Equal(1, context.ParkingOrders.Count());
            Assert.Equal(parkingOrderDto.OrderNumber, foundParkingOrder.OrderNumber);
        }

        private ParkingOrderDto GenerateParkingOrderDto()
        {
            ParkingOrderDto parkingOrderDto = new ParkingOrderDto
            {
                ParkingLotName = "No.1",
                PlateNumber = "JA8888",
            };
            return parkingOrderDto;
        }
    }
}