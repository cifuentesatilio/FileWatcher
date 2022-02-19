using folderWatcher.helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace folderWatcher
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			   .ConfigureServices((hostContext, services) =>
			   {
				   IConfiguration configuration = hostContext.Configuration;

				   readSettings options = configuration.GetSection("Watch").Get<readSettings>();

				   services.AddSingleton(options);

				   services.AddHostedService<Worker>();
			   });
	}
}
