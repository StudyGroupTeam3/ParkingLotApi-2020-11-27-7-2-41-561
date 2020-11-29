using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
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

        [Fact]
        public async Task Story1_AC3_Should_get_all_parkingLots()
        {
            // given
            var parkingLot1 = new ParkingLot("Lot1", 10, "location1");
            var parkingLot2 = new ParkingLot("Lot2", 10, "location1");
            var parkingLot3 = new ParkingLot("Lot3", 10, "location1");

            // when
            await service.AddParkingLot(parkingLot1);
            await service.AddParkingLot(parkingLot2);
            await service.AddParkingLot(parkingLot3);

            // then
            Assert.Equal(3, context.ParkingLots.CountAsync().Result);
        }

        [Fact]
        public async Task Story1_AC4_Should_get_parkingLot_by_id()
        {
            // given
            var parkingLot1 = new ParkingLot("Lot1", 10, "location1");
            var parkingLot2 = new ParkingLot("Lot2", 10, "location1");

            // when
            await service.AddParkingLot(parkingLot1);
            var name = await service.AddParkingLot(parkingLot2);
            var lot = service.GetParkingLotByName(name);

            // then
            Assert.Equal(parkingLot2, lot.Result);
        }

        [Fact]
        public async Task Story1_AC5_Should_update_parkingLot_capacity()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");
            var updateModel = new ParkingLotUpdateModel(20);

            // when
            var name = await service.AddParkingLot(parkingLot);
            await service.UpdateParkingLot(name, updateModel);
            var lot = await service.GetParkingLotByName(name);

            // then
            Assert.Equal(updateModel.Capacity, lot.Capacity);
        }
    }
}
