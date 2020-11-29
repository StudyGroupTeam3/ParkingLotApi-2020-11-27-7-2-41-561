using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    public class ParkingLotsControllerTest : TestBase
    {
        public ParkingLotsControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_parkingLot_when_add_parkingLot_with_unique_name()
        {
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/ParkingLots", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);

            Assert.Equal(parkingLotDto.Name, responseParkingLot.Name);
        }

        [Fact]
        public async Task Should_not_add_parkingLot_when_add_parkingLot_with_name_already_used()
        {
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/ParkingLots", content);

            var response = await client.PostAsync("/ParkingLots", content);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        private static ParkingLotDto GenerateParkingLotDto()
        {
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = Guid.NewGuid().ToString(),
                Capacity = 4,
                Location = "StreetAmazing",
            };
            return parkingLotDto;
        }
    }
}