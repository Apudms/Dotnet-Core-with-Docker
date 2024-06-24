using CsvHelper;
using CsvHelper.Configuration;
using WorkerServiceDemo.Models;
using Microsoft.Extensions.Options;
using System.Globalization;
using WorkerServiceDemo;

namespace ETLWorkerService
{
    internal class ETLWorkerService : BackgroundService
    {
        private readonly ILogger<ETLWorkerService> _logger;
        private readonly FileSettings _fileSettings;
        private readonly IServiceScopeFactory _scope;

        public ETLWorkerService(ILogger<ETLWorkerService> logger,
            IOptions<FileSettings> settings,
            IServiceScopeFactory scope) // DI (Dependency Injection)
        {
            _logger = logger;
            _scope = scope;
            _fileSettings = settings.Value;
            // Lazy load pattern
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("ETLWorkerService running at: {time}", DateTimeOffset.Now);
                }
                Process();
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async void Process()
        {
            try
            {
                using (var scope = _scope.CreateScope())
                {
                    // Jika terdapat file, maka simpam ke database
                    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                    var files = Directory.GetFiles(_fileSettings.CsvFolder, "*.csv");
                    foreach (var file in files)
                    {
                        using (var reader = new StreamReader(file))
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            MissingFieldFound = null,
                            HeaderValidated = null,
                            HasHeaderRecord = true,
                        }))
                        {
                            var records = csv.GetRecords<Product>();

                            // Simpan ke database
                            await db.Products.AddRangeAsync(records);
                            await db.SaveChangesAsync();
                        }
                        Task.Delay(1000);

                        // Hapus
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Processing file is failed");
            }
        }
    }
}
