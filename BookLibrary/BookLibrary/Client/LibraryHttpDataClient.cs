using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BookLibrary.Client
{
    public class LibraryHttpDataClient : ILibraryHttpDataClient
    {
        private readonly HttpClient _httpClient;

        public LibraryHttpDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetData<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            
            string json = await response.Content.ReadAsStringAsync();
            var dataServiceResponse = JsonConvert.DeserializeObject<T>(json);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return dataServiceResponse;
        }

        public async Task PostData(string url, string json)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }

        public async Task PutData(string url, string id, string json)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }        
        }

        public async Task DeleteData(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }        
        }
    }
}