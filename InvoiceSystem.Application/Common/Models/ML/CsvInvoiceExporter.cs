using CsvHelper;
using InvoiceSystem.Domain.Entities;
using System.Globalization;

namespace InvoiceSystem.Application.Common.Models.ML;

public class CsvInvoiceExporter : IInvoiceExporter
{
    public async Task ExportToCSV(string filePath)
    {
        await using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        //csv.WriteRecords(records);
    }
}
