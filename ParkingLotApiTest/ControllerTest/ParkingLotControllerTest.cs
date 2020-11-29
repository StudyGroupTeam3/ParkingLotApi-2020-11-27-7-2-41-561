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
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("TestController")]
    public class ParkingLotServiceTest : TestBase
    {
        private readonly HttpClient client;
        private readonly ParkingLotContext context;
        private readonly ParkingLotService parkingLotService;
        public ParkingLotServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            client = GetClient();
            context = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<ParkingLotContext>();
            parkingLotService = new ParkingLotService(context);
        }

        [Fact]
        public async Task Should_return_created_when_create_parkingLot_success()
        {
            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                Name = "NO.1",
                Capacity = 30,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            string httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);

            var allParkingLotsResponse = await client.GetAsync("/parkinglots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Single(returnParkingLots);
            Assert.Equal(parkingLotDto, returnParkingLots[0]);
            Assert.Equal(1, context.ParkingLot.Count());
        }

        [Fact]
        public async Task Should_return_InnerError_when_create_parkingLot_with_any_empty_field()
        {
            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                Name = string.Empty,
                Capacity = 30,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            string httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(0, context.ParkingLot.Count());
        }

        [Fact]
        public async Task Should_return_InnerError_when_create_parkingLot_with_same_name()
        {
            ParkingLotDto parkingLotDto1 = new ParkingLotDto()
            {
                Name = "NO.2",
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            ParkingLotDto parkingLotDto2 = new ParkingLotDto()
            {
                Name = "NO.2",
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            string httpContent1 = JsonConvert.SerializeObject(parkingLotDto1);
            string httpContent2 = JsonConvert.SerializeObject(parkingLotDto2);
            StringContent content1 = new StringContent(httpContent1, Encoding.UTF8, MediaTypeNames.Application.Json);
            StringContent content2 = new StringContent(httpContent2, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkinglots", content1);
            var response = await client.PostAsync("/parkinglots", content2);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(1, context.ParkingLot.Count());
        }

        [Fact]
        public async Task Should_return_204_when_delete_parkingLot_success()
        {
            ParkingLotDto parkingLotDto1 = new ParkingLotDto()
            {
                Name = "NO.1",
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            ParkingLotDto parkingLotDto2 = new ParkingLotDto()
            {
                Name = "NO.2",
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            string httpContent1 = JsonConvert.SerializeObject(parkingLotDto1);
            StringContent content1 = new StringContent(httpContent1, Encoding.UTF8, MediaTypeNames.Application.Json);
            string httpContent2 = JsonConvert.SerializeObject(parkingLotDto2);
            StringContent content2 = new StringContent(httpContent2, Encoding.UTF8, MediaTypeNames.Application.Json);
            var responsePost1 = await client.PostAsync("/parkinglots", content1);
            var responsePost2 = await client.PostAsync("/parkinglots", content2);
            var response1 = await client.DeleteAsync(responsePost1.Headers.Location);

            var allParkingLotsResponse = await client.GetAsync("/parkinglots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Equal(HttpStatusCode.NoContent, response1.StatusCode);
            Assert.Single(returnParkingLots);
            Assert.Equal(parkingLotDto2, returnParkingLots[0]);
            Assert.Equal(1, context.ParkingLot.Count());
        }

        [Fact]
        public async Task Should_get_all_parkingLots_by_pages_when_list_parkingLots()
        {
            List<ParkingLotDto> parkingLotDtos = new List<ParkingLotDto>();
            for (int i = 1; i < 16; i++)
            {
                parkingLotDtos.Add(GetParkingLotDto($"NO.{i}"));
                string httpContent = JsonConvert.SerializeObject(GetParkingLotDto($"NO.{i}"));
                StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
                await client.PostAsync("/parkinglots", content);
            }

            var allParkingLotsResponse = await client.GetAsync("/parkinglots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Equal(HttpStatusCode.OK, allParkingLotsResponse.StatusCode);
            Assert.Equal(15, returnParkingLots.Count);
            Assert.Equal(15, context.ParkingLot.Count());
        }

        [Fact]
        public async Task Should_return_200_when_click_specific_parkingLot_success()
        {
            ParkingLotDto parkingLotDto1 = new ParkingLotDto()
            {
                Name = "NO.1",
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            ParkingLotDto parkingLotDto2 = new ParkingLotDto()
            {
                Name = "NO.2",
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            string httpContent1 = JsonConvert.SerializeObject(parkingLotDto1);
            string httpContent2 = JsonConvert.SerializeObject(parkingLotDto2);
            StringContent content1 = new StringContent(httpContent1, Encoding.UTF8, MediaTypeNames.Application.Json);
            StringContent content2 = new StringContent(httpContent2, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkinglots", content1);
            var responsePost2 = await client.PostAsync("/parkinglots", content2);
            var response = await client.GetAsync(responsePost2.Headers.Location);

            var body = await response.Content.ReadAsStringAsync();
            var returnParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(parkingLotDto2, returnParkingLot);
        }

        [Fact]
        public async Task Should_Update_parkingLot_Capacity_Patch_UpdateParkingLot()
        {
            // given
            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                Name = "NO.1",
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            string httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var responsePost = await client.PostAsync("/parkinglots", content);

            // when
            UpdateParkingLotDto updateParkingLotDto = new UpdateParkingLotDto()
            {
                Name = "NO.1",
                Capacity = 10,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            string httpContentPatch = JsonConvert.SerializeObject(updateParkingLotDto);
            StringContent contentPatch = new StringContent(httpContentPatch, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PatchAsync(responsePost.Headers.Location, contentPatch);

            var body = await response.Content.ReadAsStringAsync();
            var returnParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(body);
            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateParkingLotDto.Capacity, returnParkingLot.Capacity);
            Assert.Equal(1, context.ParkingLot.Count());
        }

        private ParkingLotDto GetParkingLotDto(string name)
        {
            var parkingLotDto = new ParkingLotDto()
            {
                Name = name,
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            return parkingLotDto;
        }
    }
}