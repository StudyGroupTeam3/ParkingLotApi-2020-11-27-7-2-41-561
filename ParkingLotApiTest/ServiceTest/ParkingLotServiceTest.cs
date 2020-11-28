using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("IntegrationTest")]
    public class ParkingLotServiceTest : TestBase
    {
        public ParkingLotServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_delete_parkingLot_when_delete_parkingLot_by_name_via_parkingLotService()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            ParkingLotContext context = scopedServices.GetRequiredService<ParkingLotContext>();
            var parkingLotDto = GenerateParkingLotDto();

            ParkingLotService parkingLotService = new ParkingLotService(context);

            var parkingLotName = await parkingLotService.AddParkingLot(parkingLotDto);

            Assert.Equal(1, context.ParkingLots.Count());

            await parkingLotService.DeleteParkingLot(parkingLotName);

            Assert.Equal(0, context.ParkingLots.Count());
        }

        private static ParkingLotDto GenerateParkingLotDto()
        {
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "AmazingParking",
                Capacity = 4,
                Location = "StreetAmazing",
            };
            return parkingLotDto;
        }
    }
}