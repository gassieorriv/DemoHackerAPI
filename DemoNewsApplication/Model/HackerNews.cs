using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoNewsApplication.Model
{
    public class HackerNews : Base
    {
        private HttpClient client = new HttpClient();
        public HackerNews(IConfiguration configuration) : base(configuration) { }
        public ApiResponse<int[]> getNewStories()
        {
            string endpoint = "newstories.json";          
            ApiResponse<int[]> response = new ApiResponse<int[]>();
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(baseURL + endpoint),
                    Headers =
                    {
                       { HttpRequestHeader.Accept.ToString(), "application/json" },
                       { "X-Version", "1" }
                    },
                };

                client = new HttpClient();
                var httpResponse = client.SendAsync(httpRequestMessage).Result;
                string contentResponse = httpResponse.Content.ReadAsStringAsync().Result;
                response.data = JsonConvert.DeserializeObject<int[]>(contentResponse);
                response.isSuccessful = true;
                response.friendlyMessage = "Successfully retrieved news";
                response.errorMessage = null;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.errorMessage = ex.Message;
                response.isSuccessful = false;
                response.friendlyMessage = "Error retrieving articles. Please try again.";
            }
            return response;
        }
        public ApiResponse<Article> getStory(int id)
        {
            string endpoint = "item/" + id + ".json";
            ApiResponse<Article> response = new ApiResponse<Article>();
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(baseURL + endpoint),
                    Headers =
                    {
                       { HttpRequestHeader.Accept.ToString(), "application/json" },
                       { "X-Version", "1" }
                    },
                };

                client = new HttpClient();
                var httpResponse = client.SendAsync(httpRequestMessage).Result;
                string contentResponse = httpResponse.Content.ReadAsStringAsync().Result;
                response.data = JsonConvert.DeserializeObject<Article>(contentResponse);
                response.isSuccessful = true;
                response.friendlyMessage = "Successfully retrieved news";
                response.errorMessage = null;
            }
            catch (Exception ex)
            {
                response.data = null;
                response.errorMessage = ex.Message;
                response.isSuccessful = false;
                response.friendlyMessage = "Error retrieving articles. Please try again.";
            }
            return response;
        }
    }
}
