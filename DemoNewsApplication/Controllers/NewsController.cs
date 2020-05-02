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
using Microsoft.AspNetCore.Http;


namespace DemoNewsApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class NewsController : BaseController
    {

        public NewsController(IConfiguration configuration, IDistributedCache memoryCache) : base(configuration, memoryCache)
        {

        }

        /// <summary>
        /// Call to the Hacker News API (Brought in .net vs typescript in order to show more versatility. 
        /// As simple as the project typically would have just called from front end. While bypassing controller 
        /// methods take longer. They can provide an additional level of security (Not that this one does :-).. ) 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse<int[]> Get()
        {
            return new HackerNews(_configuration).getNewStories();
        }

        /// <summary>
        /// Retrieve cached values
        /// </summary>
        /// <param name="stories"></param>
        /// <returns></returns>
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
                        _cache.SetString(ARTICLES, json, cacheEntryOptions);
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

        /// <summary>
        /// Retrieve the cookies stored. 
        /// </summary>
        /// <param name="stories"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("StoreValueInCookie")]
        public ApiResponse<bool> StoreValueInCookie(List<Article> stories)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            try
            {
                ApiResponse<List<Article>> data = GetCookieValues();

                if (data != null && !data.isSuccessful)
                {
                    var json = JsonConvert.SerializeObject(stories.Skip(0).Take(10)); //demo amount. taking the entire 500 is too large, would need to split dataset
                    CookieOptions options = new CookieOptions()
                    { Expires = DateTime.Now.AddHours(2) };
                  
                   
                    Response.Cookies.Append(ARTICLES, json, options);
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

        /// <summary>
        /// Distributed cache to handle caching for request sent to multiple servers. Simulate an environment
        /// with a load balanced servers. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCacheValues")]
        public ApiResponse<List<Article>> GetCacheValues()
        {
            ApiResponse<List<Article>> response = new ApiResponse<List<Article>>();
            try
            {
                string articles = _cache.GetString(ARTICLES);   
                if(!string.IsNullOrEmpty(articles))
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

        /// <summary>
        /// Cookies for storing the data post session. If necessary.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCookieValues")]
        public ApiResponse<List<Article>> GetCookieValues()
        {
            ApiResponse<List<Article>> response = new ApiResponse<List<Article>>();
            try
            {
                string articles = Request.Cookies[ARTICLES];
                if (!string.IsNullOrEmpty(articles))
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