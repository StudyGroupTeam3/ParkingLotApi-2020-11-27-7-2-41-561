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
            Assert.Contains("/parkingLots/1", response.Headers.Location.AbsoluteUri);

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

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, context.ParkingLots.CountAsync().Result);
        }

        private StringContent GetRequestContent(ParkingLot parkingLot)
        {
            var httpContent = JsonConvert.SerializeObject(parkingLot);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return content;
        }
    }
}
