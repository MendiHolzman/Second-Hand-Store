using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static string CurrentPartOfTheDay()
        {
            string res = "";
            if (DateTime.Now.Hour > 5 && DateTime.Now.Hour < 12)
            {
                res = "בוקר טוב";
            }
            else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 17)
            {
                res = "צהריים טובים";
            }
            else if (DateTime.Now.Hour >= 17 && DateTime.Now.Hour < 20)
            {
                res = "ערב טוב";
            }
            else if (DateTime.Now.Hour >= 20 || DateTime.Now.Hour <= 5)
            {
                res = "לילה טוב";
            }
            return res;
        }

    }
}
