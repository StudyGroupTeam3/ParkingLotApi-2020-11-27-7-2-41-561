using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("IntegrationTest")]
    public class ParkingOrderControllerTest : TestBase
    {
        public ParkingOrderControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_parkingOrder_when_add_to_parkingLot()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = GenerateParkingLotDto();
            var httpContentOne = JsonConvert.SerializeObject(parkingLotDto);
            StringContent contentOne = new StringContent(httpContentOne, Encoding.UTF8, MediaTypeNames.Application.Json);
            var responseOne = await client.PostAsync("/ParkingLots", contentOne);
            List<ParkingOrderDto> parkingOrderDtoList = GenerateParkingOrderDtoList();
            var httpContent = JsonConvert.SerializeObject(parkingOrderDtoList[0]);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // when
            var response = await client.PostAsync("/ParkingOrders", content);

            // then
            Assert.True(response.StatusCode == HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_not_add_parkingOrder_when_add_to_parkingLot_not_exist()
        {
            // given
            var client = GetClient();
            List<ParkingOrderDto> parkingOrderDtoList = GenerateParkingOrderDtoList();
            var httpContent = JsonConvert.SerializeObject(parkingOrderDtoList[0]);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // when
            var response = await client.PostAsync("/ParkingOrders", content);

            // then
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        private static ParkingLotDto GenerateParkingLotDto()
        {
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "No.0",
                Capacity = 4,
                Location = "StreetAmazing",
            };
            return parkingLotDto;
        }

        private List<ParkingOrderDto> GenerateParkingOrderDtoList()
        {
            List<ParkingOrderDto> parkingOrderDtoList = new List<ParkingOrderDto>();
            for (var i = 0; i < 5; i++)
            {
                ParkingOrderDto parkingOrderDto = new ParkingOrderDto
                {
                    ParkingLotName = "No." + i,
                    PlateNumber = "JA888" + i,
                };
                parkingOrderDtoList.Add(parkingOrderDto);
            }

            return parkingOrderDtoList;
        }
    }
}