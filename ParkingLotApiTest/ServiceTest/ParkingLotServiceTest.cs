using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System.Threading.Tasks;
using Xunit;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("ParkingLotTest")]
    public class ParkingLotServiceTest : TestBase
    {
        private readonly ParkingLotContext context;
        private readonly ParkingLotService service;
        public ParkingLotServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            context = scopeService.GetRequiredService<ParkingLotContext>();
            service = new ParkingLotService(context);
        }

        [Fact]
        public async Task Story1_AC1_Should_add_parkingLot()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");

            // when
            await service.AddParkingLot(parkingLot);

            // then
            Assert.Equal(parkingLot, new ParkingLot(context.ParkingLots.FirstAsync().Result));
        }

        [Fact]
        public async Task Story1_AC2_Should_delete_parkingLot()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");

            // when
            var id = await service.AddParkingLot(parkingLot);
            await service.DeleteParkingLot(id);

            // then
            Assert.Equal(0, context.ParkingLots.CountAsync().Result);
        }
    }
}
