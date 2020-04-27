using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DemoNewsApplication.Model;

namespace DemoNewsApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class NewsController : BaseController
    {
        private  IConfiguration config { get; set; }
        public NewsController(IConfiguration configuration) : base(configuration)
        {
            config = configuration;
        }

        [HttpGet]
        public ApiResponse<int[]> Get()
        {
            return new HackerNews(config).getNewStories();
        }
    }
}