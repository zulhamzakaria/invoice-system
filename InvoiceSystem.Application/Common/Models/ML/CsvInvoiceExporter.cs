using CsvHelper;
using InvoiceSystem.Domain.Entities;
using System.Globalization;

namespace InvoiceSystem.Application.Common.Models.ML;

public class CsvInvoiceExporter : IInvoiceExporter
{
    public void ExportToCSV(IEnumerable<Invoice> invoiceData, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        var records = invoiceData.Select(i => { ...});
        csv.WriteRecords(records);
    }
}
