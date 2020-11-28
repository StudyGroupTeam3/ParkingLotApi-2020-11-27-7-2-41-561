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

        [Fact]
        public async Task Should_POST_Return_BadRequest_Given_Illegal_Input()
        {
            //Given
            var client = GetClient();
            ParkinglotDTO parkinglotDto = new ParkinglotDTO()
            {
                Name = string.Empty,
                Capacity = 10,
                Location = "WuDaoKong"
            };
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            //When
            var postResponse = await client.PostAsync("/ParkingLotApi/ParkingLots", content);

            //Then
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
        }
    }
}