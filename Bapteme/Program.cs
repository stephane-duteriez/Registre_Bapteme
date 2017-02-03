using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace Bapteme
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production" ?
  Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) : Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				.Build();

			host.Run();
        }
    }
}
