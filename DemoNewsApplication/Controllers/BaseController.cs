using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoNewsApplication.Controllers
{
    public class BaseController : ControllerBase
    {
        public IConfiguration _configuration { get; set; }
        public ILogger _logger { get; set; }
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


    }
}