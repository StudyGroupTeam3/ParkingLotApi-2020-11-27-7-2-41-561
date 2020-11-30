using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotApiTest
{
    public class RequestResponseContent
    {
        public async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<T>(body);

            return content;
        }

        public StringContent GetRequestContent<T>(T requestBody)
        {
            var httpContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            return content;
        }
    }
}
