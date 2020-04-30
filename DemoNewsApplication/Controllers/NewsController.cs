using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DemoNewsApplication.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DemoNewsApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class NewsController : BaseController
    {

        public NewsController(IConfiguration configuration, IDistributedCache memoryCache) : base(configuration, memoryCache)
        {

        }

        [HttpGet]
        public ApiResponse<int[]> Get()
        {
            return new HackerNews(_configuration).getNewStories();
        }

        [HttpPost]
        [Route("CacheValues")]
        public ApiResponse<bool> CacheValues(List<Article> stories)
        {
         
            ApiResponse<bool> response = new ApiResponse<bool>();
                try
                {
                ApiResponse<List<Article>> data = GetCacheValues();

                    if (data != null && !data.isSuccessful)
                    {
                        var json = JsonConvert.SerializeObject(stories);                      
                        var cacheEntryOptions = new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) };
                        _cache.SetString("articles", json, cacheEntryOptions);
                        response.data = true;
                        response.friendlyMessage = "Successfully cached records";
                        response.errorMessage = null;
                        response.isSuccessful = true;
                    }
                    else
                    {
                        response.data = true;
                        response.friendlyMessage = "Cache already exists";
                        response.isSuccessful = true;
                        response.errorMessage = null;
                    }
                }
                catch (Exception ex)
                {
                    response.data = false;
                    response.errorMessage = ex.Message;
                    response.friendlyMessage = "Error caching articles";
                    response.isSuccessful = false;
                }
            return response;
        }

        [HttpGet]
        [Route("GetCacheValues")]
        public ApiResponse<List<Article>> GetCacheValues()
        {
            ApiResponse<List<Article>> response = new ApiResponse<List<Article>>();
            try
            {
                string articles = _cache.GetString("articles");               
                response.data = JsonConvert.DeserializeObject<List<Article>>(articles);
                if (response.data != null && response.data.Count > 0)
                {
                    response.isSuccessful = true;
                    response.friendlyMessage = "Successfully retrieved cached data";
                    response.errorMessage = null;
                }
                else
                {
                    response.data = null;
                    response.errorMessage = "Could not find cached articles";
                    response.friendlyMessage = "Could not find cached articles";
                    response.isSuccessful = false;
                }
            }
            catch (Exception ex)
            {
                response.data = null;
                response.errorMessage = ex.Message;
                response.friendlyMessage = "Error retreiving cached articles";
                response.isSuccessful = false;
            }
            return response;
        }
    }
}