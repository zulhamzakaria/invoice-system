using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using InvoiceSystem.Infrastructure.MachineLearning.DataProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<IRiskTrainingDataProvider, InvoiceRiskDataProvider>();
    }).RunConsoleAsync();