using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System.Threading.Tasks;
using Xunit;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("ParkingLotTest")]
    public class OrderServiceTest : TestBase
    {
        private readonly ParkingLotContext context;
        private readonly ParkingLotService parkingLotService;
        private readonly OrderService orderService;
        public OrderServiceTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            context = scopeService.GetRequiredService<ParkingLotContext>();
            parkingLotService = new ParkingLotService(context);
            orderService = new OrderService(context);
        }

        [Fact]
        public async Task Story2_AC1_3_Should_add_order_correctly()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");
            var order = new OrderRequest("Lot1", "JA00001");

            // when
            await parkingLotService.AddParkingLot(parkingLot);
            var orderEntity = await orderService.AddOrder(order);

            // then
            Assert.Equal(orderEntity, context.Orders.FirstOrDefaultAsync().Result);
            Assert.Equal(Status.Open, context.Orders.FirstOrDefaultAsync().Result.Status);
        }
    }
}
