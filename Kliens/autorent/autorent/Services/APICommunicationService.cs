using autorent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace autorent.Services
{
    public class APICommunicationService
    {
        public readonly static string _apiUrl = "http://127.0.0.1:3000";


        public static HttpResponseMessage PostData<T>(string url, T data, string? token = null)
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
    }
}
