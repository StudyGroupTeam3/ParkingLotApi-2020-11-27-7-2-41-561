using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [Collection("IntegrationTest")]
    public class ParkingLotsControllerTest : TestBase
    {
        public ParkingLotsControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_parkingLot_when_add_parkingLot_with_unique_name()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // when
            var response = await client.PostAsync("/ParkingLots", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);

            // then
            Assert.Equal(parkingLotDto.Name, responseParkingLot.Name);
        }

        [Fact]
        public async Task Should_not_add_parkingLot_when_add_parkingLot_with_name_already_used()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/ParkingLots", content);

            // when
            var response = await client.PostAsync("/ParkingLots", content);

            // then
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_get_correct_parkingLot_when_get_parkingLot_by_name()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/ParkingLots", content);

            // when
            var response = await client.GetAsync($"/ParkingLots/{parkingLotDto.Name}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);

            // then
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Equal(parkingLotDto.Name, responseParkingLot.Name);
        }

        [Fact]
        public async Task Should_get_no_parkingLot_when_get_parkingLot_not_exist()
        {
            // given
            var client = GetClient();
            var parkingLotName = "ha";

            // when
            var response = await client.GetAsync($"/ParkingLots/{parkingLotName}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);

            // then
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Null(responseParkingLot.Name);
        }

        [Fact]
        public async Task Should_delete_parkingLot_when_delete_parkingLot_by_name()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/ParkingLots", content);
            var response = await client.GetAsync($"/ParkingLots/{parkingLotDto.Name}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);
            Assert.Equal(parkingLotDto.Name, responseParkingLot.Name);

            // when
            var deleteResponse = await client.DeleteAsync($"/ParkingLots/{parkingLotDto.Name}");
            var getResponse = await client.GetAsync($"/ParkingLots/{parkingLotDto.Name}");
            var getResponseBody = await getResponse.Content.ReadAsStringAsync();
            var responseParkingLotAfterDelete = JsonConvert.DeserializeObject<ParkingLotDto>(getResponseBody);

            // then
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.NoContent);
            Assert.Null(responseParkingLotAfterDelete.Name);
        }

        [Fact]
        public async Task Should_not_delete_parkingLot_when_delete_parkingLot_not_exist()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/ParkingLots", content);
            var response = await client.GetAsync($"/ParkingLots/{parkingLotDto.Name}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);
            Assert.Equal(parkingLotDto.Name, responseParkingLot.Name);

            // when
            var deleteResponse = await client.DeleteAsync($"/ParkingLots/{parkingLotDto.Name}");
            var getResponse = await client.GetAsync($"/ParkingLots/{parkingLotDto.Name}");
            var getResponseBody = await getResponse.Content.ReadAsStringAsync();
            var responseParkingLotAfterDelete = JsonConvert.DeserializeObject<ParkingLotDto>(getResponseBody);

            // then
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.NoContent);
            Assert.Null(responseParkingLotAfterDelete.Name);
        }

        [Fact]
        public async Task Should_update_parkingLot_capacity_when_update_parkingLot_by_name()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            UpdateParkingLotCapacityDto updateParkingLotCapacityDto = new UpdateParkingLotCapacityDto(10);
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/ParkingLots", content);
            var response = await client.GetAsync($"/ParkingLots/{parkingLotDto.Name}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);
            Assert.Equal(parkingLotDto.Capacity, responseParkingLot.Capacity);
            Assert.NotEqual(updateParkingLotCapacityDto.Capacity, responseParkingLot.Capacity);
            var httpPatchContent = JsonConvert.SerializeObject(updateParkingLotCapacityDto);
            StringContent patchContent = new StringContent(httpPatchContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // when
            var patchResponse = await client.PatchAsync($"/ParkingLots/{parkingLotDto.Name}", patchContent);
            var patchResponseBody = await patchResponse.Content.ReadAsStringAsync();
            var changedParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(patchResponseBody);

            // then
            Assert.Equal(updateParkingLotCapacityDto.Capacity, changedParkingLot.Capacity);
        }

        [Fact]
        public async Task Should_not_update_parkingLot_capacity_when_update_parkingLot_not_exist()
        {
            // given
            var client = GetClient();
            UpdateParkingLotCapacityDto updateParkingLotCapacityDto = new UpdateParkingLotCapacityDto(10);
            var httpPatchContent = JsonConvert.SerializeObject(updateParkingLotCapacityDto);
            StringContent patchContent = new StringContent(httpPatchContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var parkingLotName = "ha";

            // when
            var patchResponse = await client.PatchAsync($"/ParkingLots/{parkingLotName}", patchContent);
            var patchResponseBody = await patchResponse.Content.ReadAsStringAsync();
            var changedParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(patchResponseBody);

            // then
            Assert.Null(changedParkingLot);
        }

        [Fact]
        public async Task Should_get_parkingLots_by_page_index_when_get_parkingLots_by_page_index()
        {
            // given
            var client = GetClient();
            for (var i = 0; i < 20; i++)
            {
                ParkingLotDto parkingLotDto = GenerateParkingLotDto();
                parkingLotDto.Capacity = i;
                parkingLotDto.Location = "AmazingStreetNo." + i;
                var httpContent = JsonConvert.SerializeObject(parkingLotDto);
                StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
                await client.PostAsync("/ParkingLots", content);
            }

            var pageIndex = 1;

            // when
            var response = await client.GetAsync($"/ParkingLots/pages/{pageIndex}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var parkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(responseBody);

            // then
            Assert.Equal(15, parkingLots.Count);
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