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
using Enumerable = System.Linq.Enumerable;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingLotTest")]
    public class ParkingLotControllerTest : TestBase
    {
        private readonly ParkingLotContext context;
        public ParkingLotControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            context = scopeService.GetRequiredService<ParkingLotContext>();
        }

        [Fact]
        public async Task Story1_AC1_Should_add_parkingLot()
        {
            // given
            var client = GetClient();
            var parkingLot = new ParkingLot("Lot1", 10, "location1");

            // when
            var httpContent = JsonConvert.SerializeObject(parkingLot);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);

            // then
            Assert.Equal(parkingLot, new ParkingLot(context.ParkingLots.FirstAsync().Result));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains("/parkingLots/1", response.Headers.Location.AbsoluteUri);
        }

        [Fact]
        public async Task Story1_AC2_Should_delete_parkingLot()
        {
            // given
            var client = GetClient();
            var parkingLot = new ParkingLot("Lot1", 10, "location1");

            // when
            var httpContent = JsonConvert.SerializeObject(parkingLot);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var responseAdd = await client.PostAsync("/parkinglots", content);
            var responseFound = client.DeleteAsync(responseAdd.Headers.Location);
            var responseNotFound = client.DeleteAsync("error uri");

            // then
            Assert.Equal(HttpStatusCode.NoContent, responseFound.Result.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.Result.StatusCode);
            Assert.Equal(0, context.ParkingLots.CountAsync().Result);
        }
    }
}
