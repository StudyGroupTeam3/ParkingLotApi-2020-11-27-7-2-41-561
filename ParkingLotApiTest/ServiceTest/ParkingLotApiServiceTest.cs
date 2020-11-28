using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.DTO;
using ParkingLotApi.Repository;
using ParkingLotApi.Service;
using Xunit;
using System.Linq;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("ParkingLotTest")]
    public class ParkingLotApiServiceTest : TestBase
    {
        public ParkingLotApiServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parkinglot_success_via_parkingLotService()
        {
            //Given
            var context = GetParkingLotDbContext();
            ParkinglotDTO parkinglotDto = new ParkinglotDTO()
            {
                Name = "SuperPark_1",
                Capacity = 10,
                Location = "WuDaoKong"
            };

            ParkingLotApiService companyService = new ParkingLotApiService(context);

            //When
            var addedParkinglotID = await companyService.AddParkingLotAsnyc(parkinglotDto);
            var returnParkinglot = await companyService.GetById(addedParkinglotID);

            //Then
            Assert.Equal(parkinglotDto, returnParkinglot);
        }

        private ParkingLotContext GetParkingLotDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            ParkingLotContext context = scopedServices.GetRequiredService<ParkingLotContext>();
            return context;
        }
    }
}
