using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ParkingLotApi.Models;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingLotTest")]
    public class ParkingLotControllerTest : TestBase
    {
        private readonly ParkingLotContext context;
        private readonly HttpClient client;
        public ParkingLotControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            context = scopeService.GetRequiredService<ParkingLotContext>();
            client = GetClient();
        }

        [Fact]
        public async Task Story1_AC1_Should_add_parkingLot()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");

            // when
            var response = await client.PostAsync("/parkinglots", GetRequestContent(parkingLot));
            var responseExistName = await client.PostAsync("/parkinglots", GetRequestContent(parkingLot));

            // then
            Assert.Equal(parkingLot, new ParkingLot(context.ParkingLots.FirstAsync().Result));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains("/parkingLots/Lot1", response.Headers.Location.AbsoluteUri);

            Assert.Equal(HttpStatusCode.BadRequest, responseExistName.StatusCode);
        }

        [Fact]
        public async Task Story1_AC2_Should_delete_parkingLot()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");

            // when
            var responseAdd = await client.PostAsync("/parkinglots", GetRequestContent(parkingLot));
            var responseFound = await client.DeleteAsync(responseAdd.Headers.Location);
            var responseNotFound = await client.DeleteAsync("error uri");

            // then
            Assert.Equal(HttpStatusCode.NoContent, responseFound.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
            Assert.Equal(0, context.ParkingLots.CountAsync().Result);
        }

        [Fact]
        public async Task Story1_AC3_Should_get_all_parkingLots()
        {
            // given
            var parkingLot1 = new ParkingLot("Lot1", 10, "location1");
            var parkingLot2 = new ParkingLot("Lot2", 10, "location1");
            var parkingLot3 = new ParkingLot("Lot3", 10, "location1");

            // when
            await client.PostAsync("/parkinglots", GetRequestContent(parkingLot1));
            await client.PostAsync("/parkinglots", GetRequestContent(parkingLot2));
            await client.PostAsync("/parkinglots", GetRequestContent(parkingLot3));

            var response = await client.GetAsync("/parkinglots");
            var lots = await GetResponseContent<List<ParkingLot>>(response);

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, context.ParkingLots.CountAsync().Result);
            Assert.Equal(new List<ParkingLot>() { parkingLot3, parkingLot2, parkingLot1 }, lots);
        }

        [Fact]
        public async Task Story1_AC3_Should_return_15_most_parkingLots_each_page()
        {
            // given
            var count = 1;
            while (count < 17)
            {
                var parkingLot = new ParkingLot($"Lot{count}", 10, "location");
                await client.PostAsync("/parkinglots", GetRequestContent(parkingLot));
                count++;
            }

            // when
            var responsePage1 = await client.GetAsync("/parkingLots?page=1");
            var responsePage2 = await client.GetAsync("/parkingLots?page=2");
            var lotsInPage1 = await GetResponseContent<List<ParkingLot>>(responsePage1);
            var lotsInPage2 = await GetResponseContent<List<ParkingLot>>(responsePage2);

            // then
            Assert.Equal(HttpStatusCode.OK, responsePage1.StatusCode);
            Assert.Equal(15, lotsInPage1.Count);
            Assert.Equal(1, lotsInPage2.Count);
        }

        [Fact]
        public async Task Story1_AC4_Should_get_parkingLot_by_id()
        {
            // given
            var parkingLot1 = new ParkingLot("Lot1", 10, "location1");
            var parkingLot2 = new ParkingLot("Lot2", 10, "location1");

            // when
            await client.PostAsync("/parkinglots", GetRequestContent(parkingLot1));
            var responseAdd = await client.PostAsync("/parkinglots", GetRequestContent(parkingLot2));

            var response = await client.GetAsync(responseAdd.Headers.Location);
            var lot = await GetResponseContent<ParkingLot>(response);
            var responseNotFound = await client.GetAsync("/parkingLots/20");

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(parkingLot2, lot);

            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
        }

        [Fact]
        public async Task Story1_AC5_Should_update_parkingLot_capacity()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");
            var updateModel = new ParkingLotUpdateModel(20);

            // when
            var responseAdd = await client.PostAsync("/parkinglots", GetRequestContent(parkingLot));
            var response = await client.PatchAsync(responseAdd.Headers.Location, GetRequestContent(updateModel));
            var responseNotFound = await client.PatchAsync("error uri", GetRequestContent(updateModel));
            var responseGet = await client.GetAsync(responseAdd.Headers.Location);
            var lot = await GetResponseContent<ParkingLot>(responseGet);

            // then
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(updateModel.Capacity, lot.Capacity);

            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
        }

        private async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<T>(body);

            return content;
        }

        private StringContent GetRequestContent<T>(T requestBody)
        {
            var httpContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return content;
        }
    }
}
