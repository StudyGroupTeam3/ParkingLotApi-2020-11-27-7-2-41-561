using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingLotTest")]
    public class OrderControllerTest : TestBase
    {
        private readonly ParkingLotContext context;
        private readonly HttpClient client;
        private readonly RequestResponseContent content = new RequestResponseContent();
        public OrderControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            var scope = Factory.Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            context = scopeService.GetRequiredService<ParkingLotContext>();
            client = GetClient();
        }

        [Fact]
        public async Task Story2_AC1_3_Should_add_order_correctly()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 2, "location1");
            var order1 = new OrderRequest("Lot1", "JA00001");
            var order2 = new OrderRequest("Lot1", "JA00002");
            var order3 = new OrderRequest("Lot1", "JA00003");

            // when
            await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot));
            var response = await client.PostAsync("/orders", content.GetRequestContent(order1));
            var orderReturn = await content.GetResponseContent<Order>(response);

            var responseOrderNotClosed = await client.PostAsync("/orders", content.GetRequestContent(order1));

            await client.PostAsync("/orders", content.GetRequestContent(order2));
            var responseFullLot = await client.PostAsync("/orders", content.GetRequestContent(order3));

            // then
            Assert.Equal(orderReturn, new Order(context.Orders.FirstOrDefaultAsync().Result));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains($"/orders/{orderReturn.OrderNumber}", response.Headers.Location.AbsoluteUri);

            Assert.Equal(HttpStatusCode.BadRequest, responseOrderNotClosed.StatusCode);

            Assert.Equal(HttpStatusCode.BadRequest, responseFullLot.StatusCode);
        }

        [Fact]
        public async Task Story2_AC2_Should_update_order()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");
            var order = new OrderRequest("Lot1", "JA00001");
            var closeTime = DateTime.Now;
            var updateModel = new OrderUpdateModel(closeTime, Status.Close);

            // when
            await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot));
            var responseAddOrder = await client.PostAsync("/orders", content.GetRequestContent(order));
            var response = await client.PatchAsync(responseAddOrder.Headers.Location, content.GetRequestContent(updateModel));
            var responseNotFound = await client.PatchAsync("error uri", content.GetRequestContent(updateModel));

            // then
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(updateModel.Status, context.Orders.FirstOrDefaultAsync().Result.Status);
            Assert.Equal(closeTime, context.Orders.FirstOrDefaultAsync().Result.CloseTime);

            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
        }
    }
}
