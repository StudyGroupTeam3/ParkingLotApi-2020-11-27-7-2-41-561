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
        public async Task Should_POST_return_201_with_parking_lot_name_in_location_when_AddParkingLot()
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
            var parkingLotName = parkingLotUrl.ToString().Split('/').Last();
            Assert.Equal($"http://localhost/parkinglots/{parkingLotName}", parkingLotUrl.ToString());
            var actualParkingLotDto = new ParkingLotDto(parkingLotContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Name == parkingLotName));
            Assert.Equal(parkingLotDto, actualParkingLotDto);
        }

        [Fact]
        public async Task Should_POST_return_400_if_not_all_required_property_is_provided_when_AddParkingLot()
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
        public async Task Should_POST_return_400_if_capacity_provided_is_minus_when_AddParkingLot()
        {
            // given
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "NO.1",
                Capacity = -1,
                Location = "Area1",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);

            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_POST_return_400_parking_lot_name_already_exists_when_AddParkingLot()
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
        public async Task Should_GET_return_list_of_parking_lot_dtos_in_specified_page_range_when_GetParkingLotsByPages()
        {
            // given
            List<string> parkingLotNames = AddThreeParkingLotsIntoDB();

            // when
            var response = await client.GetAsync("/parkinglots?pageIndex=2&pageSize=2");
            response.EnsureSuccessStatusCode();

            // then
            var actualParkingLotDtos = JsonConvert.DeserializeObject<List<ParkingLotDto>>(await response.Content.ReadAsStringAsync());
            Assert.Equal(new List<ParkingLotDto>() { parkingLotDtos[2] }, actualParkingLotDtos);
        }

        [Fact]
        public async Task Should_GET_return_parking_lot_dto_specified_by_name_when_GetParkingLotsByName()
        {
            // given
            List<string> parkingLotNames = AddThreeParkingLotsIntoDB();

            // when
            var response = await client.GetAsync($"/parkinglots/{parkingLotNames[1]}");
            response.EnsureSuccessStatusCode();

            // then
            var actualParkingLotDtos = JsonConvert.DeserializeObject<ParkingLotDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(parkingLotDtos[1], actualParkingLotDtos);
        }

        [Fact]
        public async Task Should_DELETE_return_deleted_parking_lot_dto_specified_by_name_when_DeleteParkingLotsByName()
        {
            // given
            List<string> parkingLotNames = AddThreeParkingLotsIntoDB();

            // when
            var response = await client.DeleteAsync($"/parkinglots/{parkingLotNames[1]}");
            response.EnsureSuccessStatusCode();

            // then
            var actualParkingLotDtos = JsonConvert.DeserializeObject<ParkingLotDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(parkingLotDtos[1], actualParkingLotDtos);
        }

        [Fact]
        public async Task Should_DELETE_return_404_if_parking_lot_name_does_not_exist_when_DeleteParkingLotsByName()
        {
            // given
            List<string> parkingLotNames = AddThreeParkingLotsIntoDB();

            // when
            var response = await client.DeleteAsync($"/parkinglots/{parkingLotNames.Last() + 1}");

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_PATCH_return_capacity_updated_parking_lot_dto_specified_by_name_when_UpdateParkingLotCapacityByName()
        {
            // given
            List<string> parkingLotNames = AddThreeParkingLotsIntoDB();

            // when
            var content = JsonConvert.SerializeObject(new ParkingLotCapacityUpdateDto { Capacity = 20 });
            var httpContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PatchAsync($"/parkinglots/{parkingLotNames[1]}", httpContent);
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
        public async Task Should_PATCH_return_404_if_parking_lot_name_does_not_exist_when_UpdateParkingLotCapacityByName()
        {
            // given
            List<string> parkingLotNames = AddThreeParkingLotsIntoDB();

            // when
            var content = JsonConvert.SerializeObject(new ParkingLotCapacityUpdateDto { Capacity = 20 });
            var httpContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PatchAsync($"/parkinglots/{parkingLotNames.Last() + 1}", httpContent);

            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_PATCH_return_400_if_does_not_provide_capacity_when_UpdateParkingLotCapacityByName()
        {
            // given
            List<string> parkingLotNames = AddThreeParkingLotsIntoDB();

            // when
            var httpContent = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PatchAsync($"/parkinglots/{parkingLotNames[1]}", httpContent);

            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_PATCH_return_400_if_provided_capacity_is_minus_when_UpdateParkingLotCapacityByName()
        {
            // given
            List<string> parkingLotNames = AddThreeParkingLotsIntoDB();

            // when
            var content = JsonConvert.SerializeObject(new ParkingLotCapacityUpdateDto { Capacity = -1 });
            var httpContent = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PatchAsync($"/parkinglots/{parkingLotNames[1]}", httpContent);

            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private List<string> AddThreeParkingLotsIntoDB()
        {
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();
            List<string> parkingLotNames = new List<string>();
            parkingLotDtos.ForEach(async parkingLotDto => parkingLotNames.Add(await parkingLotService.AddParkingLot(parkingLotDto)));
            return parkingLotNames;
        }
    }
}