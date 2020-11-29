using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingLotContext")]
    public class ParkingLotControllerTest : TestBase
    {
        private HttpClient client;
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

        public ParkingLotControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            client = GetClient();

            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            parkingLotContext = scopedServices.GetRequiredService<ParkingLotContext>();

            parkingLotService = new ParkingLotService(parkingLotContext);
        }

        [Fact]
        public async Task Should_return_ok_with_parking_lot_id_when_POST_AddParkingLot()
        {
            // given
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "NO.1",
                Capacity = 10,
                Location = "Area1",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);
            response.EnsureSuccessStatusCode();

            // then
            int parkingLotId = int.Parse(await response.Content.ReadAsStringAsync());
            var actualParkingLotDto = new ParkingLotDto(parkingLotContext.ParkingLots.Find(parkingLotId));
            Assert.Equal(parkingLotDto, actualParkingLotDto);
        }

        [Fact]
        public async Task Should_return_400_if_not_all_required_property_is_provided_when_POST_AddParkingLot()
        {
            // given
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "NO.1",
                Capacity = 10,
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);

            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_list_of_parking_lot_dtos_in_specified_page_range_when_GET_GetParkingLotsByPages()
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
            var response = await client.GetAsync("/parkinglots?pageIndex=2&pageSize=2");
            response.EnsureSuccessStatusCode();

            // then
            var actualParkingLotDtos = JsonConvert.DeserializeObject<List<ParkingLotDto>>(await response.Content.ReadAsStringAsync());
            Assert.Equal(new List<ParkingLotDto>() { parkingLotDto3 }, actualParkingLotDtos);
        }

        [Fact]
        public async Task Should_return_parking_lot_dto_specified_By_id_when_GET_GetParkingLotsById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var response = await client.GetAsync($"/parkinglots/{parkingLotIds[1]}");
            response.EnsureSuccessStatusCode();

            // then
            var actualParkingLotDtos = JsonConvert.DeserializeObject<ParkingLotDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(parkingLotDtos[1], actualParkingLotDtos);
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