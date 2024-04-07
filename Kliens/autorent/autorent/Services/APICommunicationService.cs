using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using autorent.Properties;

namespace autorent.Services
{
    public class APICommunicationService
    {
        public readonly static string _apiUrl = new Settings().API_URL ?? "http://127.0.0.1:3000";


        public static HttpResponseMessage Post<T>(string url, T data, string? token = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    string json = JsonSerializer.Serialize(data);

                    using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        HttpResponseMessage response = client.PostAsync(url, content).Result;

                        return response;
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Valami nem jó!");
                }

            }
        }
        public static HttpResponseMessage GetData(string url, string? token = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                if (token != null)
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;


                    return response;
                }
                catch (Exception)
                {
                    throw new Exception("Valami nem jó!");
                }
            }
        }
        public static T GetObject<T>(string endpoint, string token)
        {
            HttpResponseMessage resp = APICommunicationService.GetData(endpoint, token);

            if (!resp.IsSuccessStatusCode)
            {
                switch (resp.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new Exception($"Authentikációs hiba!");
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.InternalServerError:
                        throw new Exception($"Valami nem jó!");
                }
            }

            string respDataString = resp.Content.ReadAsStringAsync().Result;
            T respData = JsonSerializer.Deserialize<T>(respDataString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return respData;
        }
        public static List<T> GetListOfObject<T>(string endpoint, string token)
        {
            HttpResponseMessage resp = APICommunicationService.GetData(endpoint, token);

            if (!resp.IsSuccessStatusCode)
            {
                switch (resp.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new Exception($"Authentikációs hiba!");
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.InternalServerError:
                        throw new Exception($"Valami nem jó!");
                }
            }

            string respDataString = resp.Content.ReadAsStringAsync().Result;
            List<T> respData = JsonSerializer.Deserialize<List<T>>(respDataString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return respData;
        }
    }
}
