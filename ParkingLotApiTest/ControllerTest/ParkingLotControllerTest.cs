using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingLotContext")]
    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_return_ok_with_parking_lot_id_when_POST_AddParkingLot()
        {
            // given
            var client = GetClient();

            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var parkingLotContext = scopedServices.GetRequiredService<ParkingLotContext>();
            parkingLotContext.Database.EnsureDeleted();
            parkingLotContext.Database.EnsureCreated();

            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "NO.1",
                Capacity = 10,
                Location = "Area1"
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);
            response.EnsureSuccessStatusCode();

            // then
            int parkingLotId = int.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(1, parkingLotId);
        }
    }
}