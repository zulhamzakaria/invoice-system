using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers;

public static class InvoiceItemMapper
{
    public static InvoiceItemDTO ToDetailsDTO(InvoiceItem item)
    {
        return new InvoiceItemDTO(item.Id, item.Description, item.Quantity, item.UnitPrice, item.TotalPrice);
    }

    public static InvoiceItemCreationDTO ToCreationDTO(InvoiceItem item)
    {
        return new InvoiceItemCreationDTO(item.Description, item.Quantity, item.UnitPrice);
    }

    public static IReadOnlyList<InvoiceItemDTO> ToListDTO(IEnumerable<InvoiceItem> items) => items.Select(ToDetailsDTO).ToList(); 
}
