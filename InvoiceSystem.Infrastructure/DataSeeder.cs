using InvoiceSystem.Domain.Entities;
using static InvoiceSystem.Domain.Entities.Address;

namespace InvoiceSystem.Infrastructure;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Companies.Any()) return; // Already seeded

        // ---------- Companies and Addresses ----------
        var company1 = new Company("Alpha Corp", "REG123");
        company1.AddAddress(new Address("123 Main St", "CityA", "StateA", "10001", "CountryA", AddressType.Billing));
        company1.AddAddress(new Address("456 Side Ave", "CityA", "StateA", "10002", "CountryA", AddressType.Shipping));

        var company2 = new Company("Beta LLC", "REG456");
        company2.AddAddress(new Address("789 Central Blvd", "CityB", "StateB", "20001", "CountryB", AddressType.Billing));
        company2.AddAddress(new Address("321 Market St", "CityB", "StateB", "20002", "CountryB", AddressType.Shipping));

        context.Companies.AddRange(company1, company2);

        // ---------- Employees ----------
        var employees = new List<Employee>
        {
            new Clerk("Clara Clerk", "clerk@company.com"),
            new FO("Frank FO1", "fo1@company.com", approvalLimit: 5000),
            new FO("Fiona FO2", "fo2@company.com", approvalLimit: 10000),
            new FO("Fred FO3", "fo3@company.com", approvalLimit: 20000),
            new FM("Mary FM", "fm@company.com")
        };
        context.Employees.AddRange(employees);

        // ---------- Invoices ----------
        var invoices = new List<Invoice>();
        var rnd = new Random();
        var companies = new[] { company1, company2 };
        var clerks = employees.OfType<Clerk>().ToList();
        var foEmployees = employees.OfType<FO>().ToList();

        for (int i = 1; i <= 10; i++)
        {
            var company = companies[rnd.Next(companies.Length)];
            var clerk = clerks[rnd.Next(clerks.Count)];

            // Snapshot addresses for this invoice
            var billingAddress = company.Addresses.First(a => a.Type == AddressType.Billing);
            var shippingAddress = company.Addresses.First(a => a.Type == AddressType.Shipping);

            var invoice = new Invoice(
                invoiceNumber: $"INV-{i:D3}",
                company: company,
                billingAddress: new Address(
                    billingAddress.Street,
                    billingAddress.City,
                    billingAddress.State,
                    billingAddress.ZipCode,
                    billingAddress.Country,
                    AddressType.Billing
                ),
                shippingAddress: new Address(
                    shippingAddress.Street,
                    shippingAddress.City,
                    shippingAddress.State,
                    shippingAddress.ZipCode,
                    shippingAddress.Country,
                    AddressType.Shipping
                ),
                invoiceDate: DateTime.UtcNow.AddDays(-i),
                createdBy: clerk
            );

            // Add items
            invoice.AddItem("Item A", rnd.Next(1, 5), rnd.Next(50, 500));
            invoice.AddItem("Item B", rnd.Next(1, 5), rnd.Next(50, 500));

            invoices.Add(invoice);
        }

        context.Invoices.AddRange(invoices);

        context.SaveChanges();
    }
}
