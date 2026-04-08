using InvoiceSystem.Application.Common.Models.ML;
using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoiceSystem.Infrastructure.MachineLearning.DataProviders;

public sealed class InvoiceRiskDataProvider : IRiskTrainingDataProvider
{
    private readonly AppDbContext _dbContext;
    ILogger<InvoiceRiskDataProvider> _logger;
    public InvoiceRiskDataProvider(AppDbContext dbContext,
        ILogger<InvoiceRiskDataProvider> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<IEnumerable<InvoiceRiskTrainingRecord>> GetTrainingDataAsync
        (CancellationToken token = default)
    {
        try
        {
            _logger.LogInformation("starting risk training data");
            var vendorStats = await GetVendorStatisticAsync(token);

            if (!vendorStats.Any())
            {
                _logger.LogWarning("No vendor stats for training data");
                return Enumerable.Empty<InvoiceRiskTrainingRecord>();
            }

            var trainingRecords = await GetRiskTrainingRecordAsync
                (vendorStats, token);

            return trainingRecords;

        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Risk training data export was canceled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export risk training data");
            throw;
        }
    }

    private async Task<IEnumerable<InvoiceRiskTrainingRecord>> GetRiskTrainingRecordAsync
        (Dictionary<Guid, VendorRiskStats> vendorStats, CancellationToken token)
    {
        var trainingRecords = new List<InvoiceRiskTrainingRecord>();

        var invoiceQuery = _dbContext.Invoices
            .Where(i => i.Status == InvoiceStatus.ApprovedByManager || i.Status == InvoiceStatus.Paid)
            .Select(i => new InvoiceProjection
            (
                i.Id,
                i.TotalAmount,
                i.Company.Id,
                i.RiskAssessment,
                i.CreatedAt
            )).AsAsyncEnumerable();

        await foreach (var invoice in invoiceQuery.WithCancellation(token))
        {
            if (!vendorStats.TryGetValue(invoice.CompanyId, out var stats))
            {
                _logger.LogWarning("No vendor stats for Company: {CompanyId}", invoice.CompanyId);
                continue;
            }

            var isHighRisk = DetermineHighRiskLabel(invoice, stats);

            var record = new InvoiceRiskTrainingRecord
            {
                Amount = (float)invoice.TotalAmount,
                VendorAverageAmount = (float)stats.AverageAmount,
                IsNewVendor = stats.InvoiceCount < 5, // Consider vendors with less than 5 invoices as new
                IsHighRisk = isHighRisk
            };

            trainingRecords.Add(record);

            if(trainingRecords.Count % 10000 == 0)
            {
                _logger.LogInformation("Processed {Count} training records", trainingRecords.Count);
            }   

        }

        return trainingRecords;

    }

    private bool DetermineHighRiskLabel(InvoiceProjection invoice, VendorRiskStats stats)
    {
        if (invoice.RiskAssessment?.RiskLevel == RiskAssessment.High)
            return true;

        if (stats.StdDev > 0 &&
            invoice.TotalAmount > stats.AverageAmount + (2 * (decimal)stats.StdDev))
            return true;

        if (invoice.TotalAmount > stats.AverageAmount * 2.5m)
            return true;

        if (invoice.TotalAmount > 100000m)
            return true;

        return false;
    }

    private async Task<Dictionary<Guid, VendorRiskStats>> GetVendorStatisticAsync(CancellationToken token)
    {
        return await _dbContext.Invoices
            .Where(i => i.Status == InvoiceStatus.ApprovedByManager || i.Status == InvoiceStatus.Paid)
            .GroupBy(i => i.Company.Id)
            .Select(g => new
            {
                CompanyId = g.Key,
                AverageAmount = g.Average(i => i.TotalAmount),
                InvoiceCount = g.Count(),
                MaxAmount = g.Max(i => i.TotalAmount),
                StdDev = EF.Functions.StandardDeviationSample(g.Select(i => (double)i.TotalAmount))
            })
            .ToDictionaryAsync(
            x => x.CompanyId,
            x => new VendorRiskStats(
                x.AverageAmount,
                x.InvoiceCount,
                x.MaxAmount,
                x.StdDev ?? 0
                ), token);
    }
}

internal record VendorRiskStats(decimal AverageAmount, int InvoiceCount, decimal MaxaAmount, double StdDev);

internal record InvoiceProjection(
        Guid Id,
        decimal TotalAmount,
        Guid CompanyId,
        InvoiceRiskAssessment RiskAssessment,
        DateTimeOffset CreatedAt);
