using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using InvoiceSystem.Infrastructure;
using InvoiceSystem.Infrastructure.MachineLearning.DataProviders;
using InvoiceSystem.MLTrainer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<MLSettings>
    (builder.Configuration.GetSection("MLSettings"));

builder.Services.AddScoped<IRiskTrainingDataProvider, InvoiceRiskDataProvider>();
builder.Services.AddScoped<IRiskModelTrainer, RiskModelTrainer>();
builder.Services.AddScoped<Runner>();

using var host = builder.Build();

var runner = host.Services.GetRequiredService<Runner>();
var exitCode = await runner.RunAsync(CancellationToken.None);

Environment.Exit(exitCode);

