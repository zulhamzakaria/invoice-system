using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Common.Models.ML;

public interface IInvoiceExporter
{
    void ExportToCSV(IEnumerable<Invoice> invoiceData, string filePath);
}
