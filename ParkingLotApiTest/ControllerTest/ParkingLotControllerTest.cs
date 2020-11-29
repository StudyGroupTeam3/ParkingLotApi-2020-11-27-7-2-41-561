using System;
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
    [Collection("TestController")]
    public class ParkingLotControllerTest : TestBase
    {
        private readonly HttpClient client;
        //private readonly StringContent content;
        //private readonly ParkingLotService parkingLotService;
        public ParkingLotControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            client = GetClient();
            client.DeleteAsync("/Companies");
            //context = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<ParkingLotContext>();
            //parkingLotService = new ParkingLotService(context);
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
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
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
        }

        //[Fact]
        //public async Task Should_get_all_parkingLots_when_list_parkingLots()
        //{
        //    ParkingLotDto parkingLotDto = new ParkingLotDto()
        //    {
        //        Name = "NO.3",
        //        Capacity = 50,
        //        Location = "BeiJingSouthRailWayStationParkingLot"
        //    };
        //    string httpContent = JsonConvert.SerializeObject(parkingLotDto);
        //    StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
        //    await client.PostAsync("/parkinglots", content);
        //    var allParkingLotsResponse = await client.GetAsync("/parkinglots");
        //    var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
        //    var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

        //    Assert.Equal(HttpStatusCode.OK, allParkingLotsResponse.StatusCode);
        //}

        [Fact]
        public async Task Should_return_204_when_delete_parkingLot_success()
        {
            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                Name = "NO.2",
                Capacity = 50,
                Location = "BeiJingSouthRailWayStationParkingLot"
            };
            string httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var responsePost = await client.PostAsync("/parkinglots", content);
            var response = await client.DeleteAsync(responsePost.Headers.Location);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
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

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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