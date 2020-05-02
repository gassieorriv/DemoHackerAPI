using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoNewsApplication.Controllers
{
    public class BaseController : Controller
    {
        public IConfiguration _configuration { get; set; }
        public ILogger _logger { get; set; }       
        public IDistributedCache _cache;
        public string ARTICLES = "articles";

        public BaseController(IConfiguration configuration, IDistributedCache memoryCache)
        {
            _configuration = configuration;
            _cache = memoryCache;
        }


    }
}