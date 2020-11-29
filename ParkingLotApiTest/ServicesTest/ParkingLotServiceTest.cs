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
        private ParkingLotService parkingLotService;
        private List<ParkingLotDto> parkingLotDtos = new List<ParkingLotDto>()
        {
            new ParkingLotDto
            {
                Name = "NO.1",
                Capacity = 10,
                Location = "Area1",
            },
            new ParkingLotDto
            {
                Name = "NO.2",
                Capacity = 10,
                Location = "Area2",
            },
            new ParkingLotDto
            {
                Name = "NO.3",
                Capacity = 10,
                Location = "Area3",
            },
        };

        public ParkingLotServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            parkingLotContext = scopedServices.GetRequiredService<ParkingLotContext>();
            parkingLotService = new ParkingLotService(parkingLotContext);
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
                Location = "Area1",
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
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var actualParkingLotDtos = await parkingLotService.GetParkingLotsByPage(2, 2);

            // then
            Assert.Equal(new List<ParkingLotDto>() { parkingLotDtos[2] }, actualParkingLotDtos);
        }

        [Fact]
        public async Task Should_return_specified_parking_lot_dto_when_GetParkingLotById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var actualParkingLotDtoNotNull = await parkingLotService.GetParkingLotById(parkingLotIds[1]);

            // then
            Assert.Equal(parkingLotDtos[1], actualParkingLotDtoNotNull);

            // when
            var actualParkingLotDtoNull = await parkingLotService.GetParkingLotById(parkingLotIds.Last() + 1);

            // then
            Assert.Null(actualParkingLotDtoNull);
        }

        [Fact]
        public async Task Should_return_specified_parking_lot_dto_when_successfully_DeleteParkingLotById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var actualParkingLotDto = await parkingLotService.DeleteParkingLotById(parkingLotIds[2]);

            // then
            Assert.Equal(parkingLotDtos[2], actualParkingLotDto);
        }

        [Fact]
        public async Task Should_return_null_if_parking_lot_with_id_does_not_exist_when_DeleteParkingLotById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var actualParkingLotDto = await parkingLotService.DeleteParkingLotById(parkingLotIds.Last() + 1);

            // then
            Assert.Null(actualParkingLotDto);
        }

        private List<int> AddThreeParkingLotsIntoDB()
        {
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();
            List<int> parkingLotIds = new List<int>();
            parkingLotDtos.ForEach(async parkingLotDto => parkingLotIds.Add(await parkingLotService.AddParkingLot(parkingLotDto)));
            return parkingLotIds;
        }
    }
}
