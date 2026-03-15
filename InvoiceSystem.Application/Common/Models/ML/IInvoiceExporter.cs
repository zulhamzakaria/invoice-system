namespace InvoiceSystem.Application.Common.Models.ML;

public interface IInvoiceExporter
{
    Task ExportToCSV(string filePath);
}
