using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoNewsApplication.Model
{
    public class Base
    {
        public IConfigurationRoot ConfigRoot;
        public string baseURL { get; set; }

        public Base(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
            baseURL = ConfigRoot["BaseURL"];
        }
    }
}
