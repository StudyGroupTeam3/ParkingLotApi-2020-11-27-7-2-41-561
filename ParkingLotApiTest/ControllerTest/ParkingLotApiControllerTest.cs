using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.DTO;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ParkingLotTest")]
    public class ParkingApiControllerTest : TestBase
    {
        public ParkingApiControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_POST_Add_Correct_Parkinglot_To_DataBase_Successfully()
        {
            //Given
            var client = GetClient();
            ParkinglotDTO parkinglotDto = new ParkinglotDTO()
            {
                Name = "SuperPark_1",
                Capacity = 10,
                Location = "WuDaoKong"
            };
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            //When
            var postResponse = await client.PostAsync("/ParkingLotApi/ParkingLots", content);
            var id = postResponse.Headers.Location.AbsolutePath.Split("/").ToList().LastOrDefault();
            var getResponse = await client.GetAsync($"/ParkingLotApi/ParkingLots/{id}");
            var body = await getResponse.Content.ReadAsStringAsync();
            var actualParkinglot = JsonConvert.DeserializeObject<ParkinglotDTO>(body);
            //Then
            Assert.Equal(parkinglotDto, actualParkinglot);
        }

        [Fact]
        public async Task Should_POST_Return_Conflict_Given_Repeated_ParkingLot_Name()
        {
            //Given
            var client = GetClient();
            ParkinglotDTO parkinglotDto = new ParkinglotDTO()
            {
                Name = "SuperPark_1",
                Capacity = 10,
                Location = "WuDaoKong"
            };
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/ParkingLotApi/ParkingLots", content);

            //When
            var postResponse = await client.PostAsync("/ParkingLotApi/ParkingLots", content);

            //Then
            Assert.Equal(HttpStatusCode.Conflict, postResponse.StatusCode);
        }

        [Theory]
        [InlineData(null, 10, "WuDaoKong")]
        [InlineData("Superpark_3", -1, "WuDaoKong")]
        [InlineData("Superpark_3", 10, null)]
        [InlineData("", 10, "WuDaoKong")]
        [InlineData("Superpark_3", 10, "")]
        public async Task Should_POST_Return_BadRequest_Given_Illegal_Input(string name, int capacity, string location)
        {
            //Given
            var client = GetClient();
            ParkinglotDTO parkinglotDto = new ParkinglotDTO()
            {
                Name = name,
                Capacity = capacity,
                Location = location
            };
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            //When
            var postResponse = await client.PostAsync("/ParkingLotApi/ParkingLots", content);

            //Then
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }

        [Fact]
        public async Task Should_DELETE_By_ID_Success_Given_Correct_ID()
        {
            //Given
            var client = GetClient();
            ParkinglotDTO parkinglotDto = new ParkinglotDTO()
            {
                Name = "SuperPark_1",
                Capacity = 10,
                Location = "WuDaoKong"
            };
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var postResponse = await client.PostAsync("/ParkingLotApi/ParkingLots", content);
            var id = postResponse.Headers.Location.AbsolutePath.Split("/").ToList().LastOrDefault();

            //When
            await client.DeleteAsync($"/ParkingLotApi/ParkingLots/{id}");
            var getAllResponse = await client.GetAsync("/ParkingLotApi/ParkingLots");
            var body = await getAllResponse.Content.ReadAsStringAsync();
            var returnParkinglots = JsonConvert.DeserializeObject<List<ParkinglotDTO>>(body);

            //Then
            Assert.Equal(0, returnParkinglots.Count);
        }

        [Fact]
        public async Task Should_DELETE_By_ID_Return_NotFound_Given_Wrong_ID()
        {
            //Given
            var client = GetClient();
            var noneExistId = "31";

            //When
            var response = await client.DeleteAsync($"/ParkingLotApi/ParkingLots/{noneExistId}");

            //Then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}