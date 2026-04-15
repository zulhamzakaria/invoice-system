using CsvHelper;
using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace InvoiceSystem.MLTrainer;

public interface ICSVExporter
{
    Task<int> ExportToCsvAsync(string filePath, CancellationToken ct);
}
sealed class CSVExporter(
    IRiskTrainingDataProvider dataProvider, 
    ILogger<CSVExporter> logger) 
    : ICSVExporter
{
    public async Task<int> ExportToCsvAsync(string filePath, CancellationToken ct)
    {
        logger.LogInformation("Exporting data to CSV file at {FilePath}", filePath);

        var count = 0;
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        await foreach(var record in dataProvider.GetTrainingDataStream(ct))
        {
            csv.WriteRecord(record);
            await csv.NextRecordAsync();
            count++;
        }

        logger.LogInformation("Finished exporting {Count} records to CSV file at {FilePath}", count, filePath);
        return count;

    }
}
