﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PMB.Bot.Schedules;
using PMB.Config;
using System;
using System.Threading.Tasks;

namespace PMB.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => { })
                .ConfigureServices(async services =>
              {
                  services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
                  new DependencyInjectionConfig().Set(services);
                  services.AddScoped<IGetPriceSchedule, GetPriceSchedule>();

                  var serviceProvider = services.BuildServiceProvider();
                  var getPriceService = serviceProvider.GetRequiredService<IGetPriceSchedule>();
                  getPriceService.Start();

              });

            var buildHost = host.Build();
            buildHost.Run();
        }
    }
}
