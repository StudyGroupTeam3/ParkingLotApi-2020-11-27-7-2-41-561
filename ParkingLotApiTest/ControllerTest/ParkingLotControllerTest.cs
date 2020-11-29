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
        public async Task Should_return_201_with_parking_lot_id_in_location_when_POST_AddParkingLot()
        {
            // given
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();
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
            var parkingLotUrl = response.Headers.Location;
            var parkingLotId = int.Parse(parkingLotUrl.ToString().Split('/').Last());
            Assert.Equal($"http://localhost/parkinglots/{parkingLotId}", parkingLotUrl.ToString());
            var actualParkingLotDto = new ParkingLotDto(parkingLotContext.ParkingLots.Find(parkingLotId));
            Assert.Equal(parkingLotDto, actualParkingLotDto);
        }

        [Fact]
        public async Task Should_return_400_if_not_all_required_property_is_provided_when_POST_AddParkingLot()
        {
            // given
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();
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
        public async Task Should_return_400_parking_lot_name_already_exists_when_POST_AddParkingLot()
        {
            // given
            AddThreeParkingLotsIntoDB();
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

            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Parking lot name already exists!", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Should_return_list_of_parking_lot_dtos_in_specified_page_range_when_GET_GetParkingLotsByPages()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var response = await client.GetAsync("/parkinglots?pageIndex=2&pageSize=2");
            response.EnsureSuccessStatusCode();

            // then
            var actualParkingLotDtos = JsonConvert.DeserializeObject<List<ParkingLotDto>>(await response.Content.ReadAsStringAsync());
            Assert.Equal(new List<ParkingLotDto>() { parkingLotDtos[2] }, actualParkingLotDtos);
        }

        [Fact]
        public async Task Should_return_parking_lot_dto_specified_by_id_when_GET_GetParkingLotsById()
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

        [Fact]
        public async Task Should_return_deleted_parking_lot_dto_specified_by_id_when_DELETE_DeleteParkingLotsById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var response = await client.DeleteAsync($"/parkinglots/{parkingLotIds[1]}");
            response.EnsureSuccessStatusCode();

            // then
            var actualParkingLotDtos = JsonConvert.DeserializeObject<ParkingLotDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(parkingLotDtos[1], actualParkingLotDtos);
        }

        [Fact]
        public async Task Should_return_404_if_parking_lot_id_does_not_exist_when_DELETE_DeleteParkingLotsById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var response = await client.DeleteAsync($"/parkinglots/{parkingLotIds.Last() + 1}");

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_capacity_updated_parking_lot_dto_specified_by_id_when_PATCH_UpdateParkingLotCapacityById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var content = JsonConvert.SerializeObject(new ParkingLotCapacityUpdateDto { Capacity = 20 });
            var httpContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PatchAsync($"/parkinglots/{parkingLotIds[1]}", httpContent);
            response.EnsureSuccessStatusCode();
            var expactedParkingLotDto = new ParkingLotDto
            {
                Name = parkingLotDtos[1].Name,
                Capacity = 20,
                Location = parkingLotDtos[1].Location,
            };

            // then
            var actualParkingLotDtos = JsonConvert.DeserializeObject<ParkingLotDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(expactedParkingLotDto, actualParkingLotDtos);
        }

        [Fact]
        public async Task Should_return_404_if_parking_lot_id_does_not_exist_when_PATCH_UpdateParkingLotCapacityById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var content = JsonConvert.SerializeObject(new ParkingLotCapacityUpdateDto { Capacity = 20 });
            var httpContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PatchAsync($"/parkinglots/{parkingLotIds.Last() + 1}", httpContent);

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_400_if_does_not_provided_capacity_when_PATCH_UpdateParkingLotCapacityById()
        {
            // given
            List<int> parkingLotIds = AddThreeParkingLotsIntoDB();

            // when
            var httpContent = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PatchAsync($"/parkinglots/{parkingLotIds[1]}", httpContent);

            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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