using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.API.Helper
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;
        
        public static void Initialize(IConfiguration configuration) 
        {
         _configuration = configuration;    
        }
        public static IConfiguration GetConfiguration() 
        { 
         return _configuration;
        }
    }
}
