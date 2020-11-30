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
    public class ParkingOrderServiceTest : TestBase
    {
        private ParkingLotContext parkingLotContext;
        private ParkingOrderService parkService;

        public ParkingOrderServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            parkingLotContext = scopedServices.GetRequiredService<ParkingLotContext>();
            parkService = new ParkingOrderService(parkingLotContext);
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
            parkingLots.ForEach(parkingLot =>
            {
                parkingLotContext.ParkingLots.Add(parkingLot);
                parkingLotContext.SaveChanges();
            });
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
            parkingLots.ForEach(parkingLot =>
            {
                parkingLotContext.ParkingLots.Add(parkingLot);
                parkingLotContext.SaveChanges();
            });
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

        [Fact]
        public async Task Should_change_parking_order_status_to_false_when_Leave()
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
            parkingLots.ForEach(parkingLot =>
            {
                parkingLotContext.ParkingLots.Add(parkingLot);
                parkingLotContext.SaveChanges();
            });
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
            parkingOrders.ForEach(parkingOrder =>
            {
                parkingLotContext.ParkingOrders.Add(parkingOrder);
                parkingLotContext.SaveChanges();
            });
            var parkingOrderDto = new ParkingOrderDto
            {
                NameOfParkingLot = "NO.1",
                PlateNumber = "ABC000",
            };
            // when
            await parkService.Leave(parkingOrderDto);
            // then
            var matchedParkingOrderClosed = parkingLotContext.ParkingOrders.Where(
              parkingOrder => parkingOrder.NameOfParkingLot == parkingOrderDto.NameOfParkingLot &&
                              parkingOrder.PlateNumber == parkingOrderDto.PlateNumber &&
                              parkingOrder.OrderStatus == false &&
                              parkingOrder.CloseTime > parkingOrder.CreationTime).ToList();
            Assert.Single(matchedParkingOrderClosed);
            var matchedParkingOrderOpen = parkingLotContext.ParkingOrders.Where(
              parkingOrder => parkingOrder.NameOfParkingLot == parkingOrderDto.NameOfParkingLot &&
                              parkingOrder.PlateNumber == parkingOrderDto.PlateNumber &&
                              parkingOrder.OrderStatus == true).ToList();
            Assert.Empty(matchedParkingOrderOpen);
        }
    }
}
