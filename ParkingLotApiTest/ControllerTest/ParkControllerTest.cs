using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingLotContext")]
    public class ParkControllerTest : TestBase
    {
        private HttpClient client;
        private ParkingLotContext parkingLotContext;
        private ParkService parkService;

        public ParkControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            client = GetClient();

            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            parkingLotContext = scopedServices.GetRequiredService<ParkingLotContext>();

            parkService = new ParkService(parkingLotContext);
        }

        [Fact]
        public async Task Should_return_created_parking_order_when_POST_park_car()
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

            var parkingOrderDto = new ParkingOrderDto
            {
                NameOfParkingLot = "NO.1",
                PlateNumber = "ABC000",
            };
            // when
            var httpContent = JsonConvert.SerializeObject(parkingOrderDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parking", content);
            response.EnsureSuccessStatusCode();

            // then
            var actualParkingOrderDto = JsonConvert.DeserializeObject<ParkingOrderDto>(await response.Content.ReadAsStringAsync());
            var matchedParkingOrder = parkingLotContext.ParkingOrders.Where(
                parkingOrder => parkingOrder.NameOfParkingLot == actualParkingOrderDto.NameOfParkingLot &&
                                parkingOrder.PlateNumber == actualParkingOrderDto.PlateNumber &&
                                parkingOrder.OrderStatus == true).ToList();
            Assert.Single(matchedParkingOrder);
        }

        [Fact]
        public async Task Should_return_400_and_no_parking_order_be_created_when_POST_there_is_no_free_space_in_parking_lot()
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
            var httpContent = JsonConvert.SerializeObject(parkingOrderDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parking", content);

            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(2, parkingLotContext.ParkingOrders.Count());
        }
    }
}
