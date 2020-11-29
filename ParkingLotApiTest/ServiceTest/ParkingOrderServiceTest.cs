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
        public async Task Should_add_parkingOrder_when_add_parkingOrder_via_parkingOrderService()
        {
            // given
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            ParkingLotContext context = scopedServices.GetRequiredService<ParkingLotContext>();
            var parkingOrderDto = GenerateParkingOrderDto();
            ParkingOrderService parkingOrderService = new ParkingOrderService(context);

            // when
            var parkingOrderNumber = await parkingOrderService.AddParkingOrder(parkingOrderDto);
            var foundParkingOrder = await context.ParkingOrders.FirstOrDefaultAsync(parkingOrderEntity => parkingOrderEntity.OrderNumber == parkingOrderNumber);

            // then
            Assert.Equal(1, context.ParkingOrders.Count());
            Assert.Equal(parkingOrderDto.OrderNumber, foundParkingOrder.OrderNumber);
        }

        [Fact]
        public async Task Should_get_all_parkingOrders_when_get_parkingOrders_via_parkingOrderService()
        {
            // given
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            ParkingLotContext context = scopedServices.GetRequiredService<ParkingLotContext>();
            ParkingOrderService parkingOrderService = new ParkingOrderService(context);
            var parkingOrderDtoList = GenerateParkingOrderDtoList();
            foreach (var parkingOrderDto in parkingOrderDtoList)
            {
                await parkingOrderService.AddParkingOrder(parkingOrderDto);
            }

            // when
            var allParkingOrders = await parkingOrderService.GetAllParkingOrders();

            // then
            Assert.Equal(5, allParkingOrders.Count());
        }

        [Fact]
        public async Task Should_get_correct_parkingOrder_when_get_parkingOrder_via_parkingOrderService()
        {
            // given
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            ParkingLotContext context = scopedServices.GetRequiredService<ParkingLotContext>();
            ParkingOrderService parkingOrderService = new ParkingOrderService(context);
            var parkingOrderDtoList = GenerateParkingOrderDtoList();
            foreach (var parkingOrderDto in parkingOrderDtoList)
            {
                await parkingOrderService.AddParkingOrder(parkingOrderDto);
            }

            // when
            var parkingOrder = await parkingOrderService.GetParkingOrderByOrderNumber(parkingOrderDtoList[0].OrderNumber);

            // then
            Assert.Equal(parkingOrderDtoList[0], parkingOrder);
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

        private List<ParkingOrderDto> GenerateParkingOrderDtoList()
        {
            List<ParkingOrderDto> parkingOrderDtoList = new List<ParkingOrderDto>();
            for (var i = 0; i < 5; i++)
            {
                ParkingOrderDto parkingOrderDto = new ParkingOrderDto
                {
                    ParkingLotName = "No." + i,
                    PlateNumber = "JA888" + i,
                };
                parkingOrderDtoList.Add(parkingOrderDto);
            }

            return parkingOrderDtoList;
        }
    }
}