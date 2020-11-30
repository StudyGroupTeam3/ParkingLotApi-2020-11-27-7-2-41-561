﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingLotTest")]
    public class ParkingLotControllerTest : TestBase
    {
        private readonly ParkingLotContext context;
        private readonly HttpClient client;
        private readonly RequestResponseContent content = new RequestResponseContent();
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
            var response = await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot));
            var responseExistName = await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot));

            // then
            Assert.Equal(parkingLot, new ParkingLot(context.ParkingLots.FirstAsync().Result));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains("/parkingLots/Lot1", response.Headers.Location.AbsoluteUri);

            Assert.Equal(HttpStatusCode.BadRequest, responseExistName.StatusCode);
        }

        [Fact]
        public async Task Story1_AC2_Should_delete_parkingLot()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");

            // when
            var responseAdd = await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot));
            var responseFound = await client.DeleteAsync(responseAdd.Headers.Location);
            var responseNotFound = await client.DeleteAsync("error uri");

            // then
            Assert.Equal(HttpStatusCode.NoContent, responseFound.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
            Assert.Equal(0, context.ParkingLots.CountAsync().Result);
        }

        [Fact]
        public async Task Story1_AC3_Should_return_15_most_parkingLots_each_page()
        {
            // given
            var count = 1;
            while (count < 17)
            {
                var parkingLot = new ParkingLot($"Lot{count}", 10, "location");
                await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot));
                count++;
            }

            // when
            var responsePage1 = await client.GetAsync("/parkingLots?page=1");
            var responsePage2 = await client.GetAsync("/parkingLots?page=2");
            var responsePage3 = await client.GetAsync("/parkingLots?page=0");
            var lotsInPage1 = await content.GetResponseContent<List<string>>(responsePage1);
            var lotsInPage2 = await content.GetResponseContent<List<string>>(responsePage2);

            // then
            Assert.Equal(HttpStatusCode.OK, responsePage1.StatusCode);
            Assert.Equal(15, lotsInPage1.Count);
            Assert.Equal(1, lotsInPage2.Count);

            Assert.Equal(HttpStatusCode.BadRequest, responsePage3.StatusCode);
        }

        [Fact]
        public async Task Story1_AC4_Should_get_parkingLot_by_id()
        {
            // given
            var parkingLot1 = new ParkingLot("Lot1", 10, "location1");
            var parkingLot2 = new ParkingLot("Lot2", 10, "location1");

            // when
            await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot1));
            var responseAdd = await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot2));

            var response = await client.GetAsync(responseAdd.Headers.Location);
            var lot = await content.GetResponseContent<ParkingLot>(response);
            var responseNotFound = await client.GetAsync("/parkingLots/20");

            // then
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(parkingLot2, lot);

            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
        }

        [Fact]
        public async Task Story1_AC5_Should_update_parkingLot_capacity()
        {
            // given
            var parkingLot = new ParkingLot("Lot1", 10, "location1");
            var updateModel = new ParkingLotUpdateModel(20);

            // when
            var responseAdd = await client.PostAsync("/parkinglots", content.GetRequestContent(parkingLot));
            var response = await client.PatchAsync(responseAdd.Headers.Location, content.GetRequestContent(updateModel));
            var responseNotFound = await client.PatchAsync("error uri", content.GetRequestContent(updateModel));
            var responseGet = await client.GetAsync(responseAdd.Headers.Location);
            var lot = await content.GetResponseContent<ParkingLot>(responseGet);

            // then
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(updateModel.Capacity, lot.Capacity);

            Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
        }
    }
}
