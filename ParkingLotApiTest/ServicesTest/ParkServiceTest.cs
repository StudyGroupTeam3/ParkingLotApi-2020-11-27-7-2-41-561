using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using Xunit;

namespace ParkingLotApiTest.ServicesTest
{
    [Collection("ParkingLotContext")]
    public class ParkServiceTest : TestBase
    {
        private ParkingLotContext parkingLotContext;
        private ParkService parkService;

        public ParkServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            parkingLotContext = scopedServices.GetRequiredService<ParkingLotContext>();
            parkService = new ParkService(parkingLotContext);
        }

        [Fact]
        public async Task Should_return_true_when_there_is_free_space_in_parking_lot_specified_by_name()
        {
            // given
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();
            List<ParkingLotEntity> parkingLots = new List<ParkingLotEntity>()
            {
                new ParkingLotEntity
                {
                    Name = "NO.1",
                    Capacity = 2,
                    Location = "Area1",
                },
            };

            List<ParkingOrderEntity> parkingOrders = new List<ParkingOrderEntity>()
            {
                new ParkingOrderEntity
                {
                    NameOfParkingLot = "NO.1",
                    PlateNumber = "ABC000",
                    CreationTime = DateTime.Now,
                    OrderStatus = false,
                },
                new ParkingOrderEntity
                {
                    NameOfParkingLot = "NO.1",
                    PlateNumber = "ABC001",
                    CreationTime = DateTime.Now,
                    OrderStatus = false,
                },
            };
            parkingLots.ForEach(parkingLot =>
            {
                parkingLotContext.ParkingLots.Add(parkingLot);
                parkingLotContext.SaveChanges();
            });
            parkingOrders.ForEach(parkingOrder =>
            {
                parkingLotContext.ParkingOrders.Add(parkingOrder);
                parkingLotContext.SaveChanges();
            });

            // when
            var actual = parkService.IsFreeSpaceInParkingLot("NO.1");
            // then
            Assert.True(actual);
        }

        [Fact]
        public async Task Should_return_false_when_there_is_no_free_space_in_parking_lot_specified_by_name()
        {
            // given
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();
            List<ParkingLotEntity> parkingLots = new List<ParkingLotEntity>()
            {
                new ParkingLotEntity
                {
                    Name = "NO.1",
                    Capacity = 2,
                    Location = "Area1",
                },
            };

            List<ParkingOrderEntity> parkingOrders = new List<ParkingOrderEntity>()
            {
                new ParkingOrderEntity
                {
                    NameOfParkingLot = "NO.1",
                    PlateNumber = "ABC000",
                    CreationTime = DateTime.Now,
                    OrderStatus = true,
                },
                new ParkingOrderEntity
                {
                    NameOfParkingLot = "NO.1",
                    PlateNumber = "ABC001",
                    CreationTime = DateTime.Now,
                    OrderStatus = true,
                },
            };
            parkingLots.ForEach(parkingLot =>
            {
                parkingLotContext.ParkingLots.Add(parkingLot);
                parkingLotContext.SaveChanges();
            });
            parkingOrders.ForEach(parkingOrder =>
            {
                parkingLotContext.ParkingOrders.Add(parkingOrder);
                parkingLotContext.SaveChanges();
            });
            // when
            var actual = parkService.IsFreeSpaceInParkingLot("NO.1");
            // then
            Assert.False(actual);
            parkingOrders[1].OrderStatus = false;
        }

        [Fact]
        public async Task Should_create_in_system_and_return_new_parking_order_when_ParkCar()
        {
            // given
            var parkingOrderDto = new ParkingOrderDto
            {
                NameOfParkingLot = "NO.1",
                PlateNumber = "ABC000",
            };
            // when
            var actualParkingOrderDto = await parkService.ParkCar(parkingOrderDto);
            // then
            var matchedParkingOrder = parkingLotContext.ParkingOrders.Where(
                parkingOrder => parkingOrder.NameOfParkingLot == actualParkingOrderDto.NameOfParkingLot &&
                                parkingOrder.PlateNumber == actualParkingOrderDto.PlateNumber &&
                                parkingOrder.OrderStatus == true).ToList();
            Assert.Single(matchedParkingOrder);
        }
    }
}
