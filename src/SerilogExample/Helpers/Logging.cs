using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace SerilogExample.Helpers
{
    public class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureSerilog =>
           (hostingContext, loggerConfiguration) =>
           {
               // You can also configure these by configuration file : https://github.com/serilog/serilog-settings-configuration
               var env = hostingContext.HostingEnvironment;
               
               loggerConfiguration
               .MinimumLevel.Information()
               .ReadFrom.Configuration(hostingContext.Configuration)
               .Enrich.FromLogContext()
               .Enrich.WithExceptionDetails()
               .Enrich.WithProperty("ApplicationName", env.ApplicationName)
               .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
               .WriteTo.Async(a =>
               {
                   a.Console();
               });

               if (hostingContext.HostingEnvironment.IsDevelopment())
               {
                   // You can override like 
                   //loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               }

               var seqUrl = hostingContext.Configuration.GetValue<string>("Logging:SeqUrl");

               if (!string.IsNullOrEmpty(seqUrl))
               {
                   loggerConfiguration.WriteTo.Seq(seqUrl);
               }
           };
    }
}