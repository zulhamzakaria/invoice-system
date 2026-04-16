using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using InvoiceSystem.Infrastructure.MachineLearning.DataProviders;
using InvoiceSystem.MLTrainer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<MLSettings>
    (builder.Configuration.GetSection("MLSettings"));
builder.Services.AddScoped<Runner>();


await Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<IRiskTrainingDataProvider, InvoiceRiskDataProvider>();
    }).RunConsoleAsync();