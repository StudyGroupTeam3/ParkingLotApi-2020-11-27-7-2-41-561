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
            var responseFound = client.DeleteAsync(responseAdd.Headers.Location);
            var responseNotFound = client.DeleteAsync("error uri");

            // then
            Assert.Equal(HttpStatusCode.NoContent, responseFound.Result.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.Result.StatusCode);
            Assert.Equal(0, context.ParkingLots.CountAsync().Result);
        }

        private StringContent GetRequestContent(ParkingLot parkingLot)
        {
            var httpContent = JsonConvert.SerializeObject(parkingLot);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return content;
        }
    }
}
