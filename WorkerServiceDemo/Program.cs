using WorkerServiceDemo.Models;
using Microsoft.EntityFrameworkCore;
using WorkerServiceDemo;

var builder = Host.CreateApplicationBuilder(args);

// Baca config
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// baca connection string
var connectionString = configuration.GetConnectionString("ConnStr");

// DI (Dependency Injection)
// var services = new ServiceCollection()
//     .AddDbContext<ProductDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(connectionString));

// DI File Settings
//var fileSettings = configuration.GetSection("FileSettings").Get<FileSettings>();
builder.Services.Configure<FileSettings>(configuration.GetSection("FileSettings"));

builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<ETLWorkerService.ETLWorkerService>();

var host = builder.Build();
host.Run();
