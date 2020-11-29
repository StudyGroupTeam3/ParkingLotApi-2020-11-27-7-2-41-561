using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using Xunit;

namespace ParkingLotApiTest
{
    [Collection("ParkingLotContext")]
    public class ParkingLotServiceTest : TestBase
    {
        private ParkingLotContext parkingLotContext;

        public ParkingLotServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            parkingLotContext = scopedServices.GetRequiredService<ParkingLotContext>();
        }

        [Fact]
        public async Task Should_add_new_parking_lot_into_database_and_return_id_when_AddParkingLot()
        {
            // given
            var parkingLotService = new ParkingLotService(parkingLotContext);

            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "NO.1",
                Capacity = 10,
                Location = "Area1"
            };

            // when
            int parkingLotId = await parkingLotService.AddParkingLot(parkingLotDto);
            var actualParkingLotDto = new ParkingLotDto(parkingLotContext.ParkingLots.Find(parkingLotId));

            // then
            Assert.Equal(parkingLotDto, actualParkingLotDto);
        }

        [Fact]
        public async Task Should_return_list_of_parking_lot_dtos_in_specified_page_range_when_GetParkingLotsByPage()
        {
            // given
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();
            var parkingLotService = new ParkingLotService(parkingLotContext);

            ParkingLotDto parkingLotDto1 = new ParkingLotDto
            {
                Name = "NO.1",
                Capacity = 10,
                Location = "Area1"
            };
            await parkingLotService.AddParkingLot(parkingLotDto1);

            ParkingLotDto parkingLotDto2 = new ParkingLotDto
            {
                Name = "NO.2",
                Capacity = 10,
                Location = "Area2"
            };
            await parkingLotService.AddParkingLot(parkingLotDto2);

            ParkingLotDto parkingLotDto3 = new ParkingLotDto
            {
                Name = "NO.3",
                Capacity = 10,
                Location = "Area3"
            };
            await parkingLotService.AddParkingLot(parkingLotDto3);

            // when
            var actualParkingLotDtos = await parkingLotService.GetParkingLotsByPage(2, 2);

            // then
            Assert.Equal(new List<ParkingLotDto>() { parkingLotDto3 }, actualParkingLotDtos);
        }
    }
}
