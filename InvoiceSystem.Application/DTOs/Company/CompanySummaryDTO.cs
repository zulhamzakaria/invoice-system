using System.Text.Json.Serialization;

namespace InvoiceSystem.Application.DTOs.Company;

public record CompanySummaryDTO(Guid Id, 
                                [property: JsonPropertyName("company_name")]
                                string Name, 
                                string RegistrationNumber);
