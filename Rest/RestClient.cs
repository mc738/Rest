using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rest
{
    public class RestClient
    {
        private const string BaseUrl = "https://workrapi.somerandomdomain.co.uk/api/";
        private HttpClient client { get; set; } = new HttpClient();


        public void SetAuthToken(string token)
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task<string> Post<T>(string url, T content)
        {

            var json = JsonSerializer.Serialize(content);

            var sContent = new StringContent(json);

            sContent.Headers.ContentType.MediaType = "application/json";

            var resposne = await client.PostAsync($"{BaseUrl}{url}", sContent);

            var responseContent = await resposne.Content.ReadAsStringAsync();

            if (!resposne.IsSuccessStatusCode)
                throw new HttpRequestException($"{resposne.StatusCode} - {responseContent}");

            return responseContent;
        }

        public async Task<T> Get<T>(string url)
        {
            //var json = JsonConvert.SerializeObject(content);

            var resposne = await client.GetAsync($"{BaseUrl}{url}");

            var responseContent = await resposne.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseContent);
        }
    }
}
