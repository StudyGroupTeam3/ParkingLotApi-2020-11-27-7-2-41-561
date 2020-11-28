using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using Xunit;

namespace ParkingLotApiTest
{
    [Collection("ParkingLotContext")]
    public class ParkingLotServiceTest : TestBase
    {
        public ParkingLotServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_new_parking_lot_into_database_and_return_id_when_AddParkingLot()
        {
            // given
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var parkingLotContext = scopedServices.GetRequiredService<ParkingLotContext>();

            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "NO.1",
                Capacity = 10,
                Location = "Area1"
            };

            // when
            var parkingLotService = new ParkingLotService(parkingLotContext);

            // then
            int parkingLotId = await parkingLotService.AddParkingLot(parkingLotDto);
            Assert.Equal(1, parkingLotId);
        }
    }
}
