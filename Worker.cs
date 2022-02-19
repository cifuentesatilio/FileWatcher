using System;
using System.Threading;
using System.Threading.Tasks;
using folderWatcher.helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace folderWatcher
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly readSettings _option;

		public Worker(ILogger<Worker> logger, readSettings options)
		{
			_logger = logger;
			_option = options;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			var _validate = new validateFile(_option);
			_validate.validateCreateRejectingFolder(_option.pathToWatch);
			_validate.validateCreateProcessingFolder(_option.pathToWatch);

			return base.StartAsync(cancellationToken);
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				dirMonitor _monitor = new dirMonitor(_option);

				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}
