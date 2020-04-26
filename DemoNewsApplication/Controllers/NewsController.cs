using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoNewsApplication.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(IConfiguration configuration, ILogger logger) : base(configuration, logger)
        {  }
    }
}