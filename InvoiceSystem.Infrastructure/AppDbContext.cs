

using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InvoiceSystem.Infrastructure;
public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<WorkflowStep> WorkflowSteps { get; set; }

    public DbSet<LoadTracker> LoadTrackers { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureEmployees(modelBuilder);
        ConfigureInvoices(modelBuilder);
        ConfigureCompanies(modelBuilder);
        ConfigureWorkflowSteps(modelBuilder);
        ConfigureLoadTracker(modelBuilder);
    }

    private void ConfigureLoadTracker(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoadTracker>(tracker =>
        {

            tracker.HasKey(e => e.Id);
            tracker.HasIndex(e => e.ApproverId).IsUnique();

            tracker.HasOne(lt => lt.Approver)
                .WithMany()
                .HasForeignKey(lt => lt.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);

        });
    }

    private void ConfigureWorkflowSteps(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkflowStep>(step =>
        {
            step.HasKey(wfs => wfs.Id);

            step.Property(wfs => wfs.ActionType).HasConversion<string>().IsRequired();
            step.Property(wfs => wfs.StatusBefore).HasConversion<string>().IsRequired();
            step.Property(wfs => wfs.StatusAfter).HasConversion<string>().IsRequired();

            step.Property(wfs => wfs.Reason).IsRequired().HasMaxLength(500);

            step.Property(wfs => wfs.Timestamp).IsRequired();

            step.HasOne<Invoice>()
            .WithMany()
            .HasForeignKey(wfs => wfs.InvoiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            step.HasIndex(wfs => wfs.InvoiceId);

            step.HasIndex(wfs => new { wfs.InvoiceId, wfs.Timestamp }).IsUnique(false);
        });
        modelBuilder.Entity<Invoice>().HasIndex(i => i.InvoiceNumber).IsUnique();
    }

    private void ConfigureEmployees(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasDiscriminator<string>("EmployeeType")
            .HasValue<Clerk>("Clerk")
            .HasValue<FO>("FO")
            .HasValue<FM>("FM");

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();
    }

    private void ConfigureInvoices(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>()
            .Property(i => i.Status)
            .HasConversion<string>(); // store enum as string

        modelBuilder.Entity<Invoice>()
            .HasMany<InvoiceItem>("_items")
            .WithOne(i => i.Invoice)
            .HasForeignKey(i => i.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade); // cascade delete

        modelBuilder.Entity<Invoice>()
            .Navigation("_items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.CreatedBy)
            .WithMany()
            .HasForeignKey(i => i.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.ApprovedBy)
            .WithMany()
            .HasForeignKey(i => i.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Invoice>()
            .Navigation("_items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<Invoice>()
       .OwnsOne(i => i.BillingAddress, a =>
       {
           a.Property(p => p.Street).HasColumnName("BillingStreet");
           a.Property(p => p.City).HasColumnName("BillingCity");
           a.Property(p => p.State).HasColumnName("BillingState");
           a.Property(p => p.ZipCode).HasColumnName("BillingZipCode");
           a.Property(p => p.Country).HasColumnName("BillingCountry");
       });

        modelBuilder.Entity<Invoice>()
            .OwnsOne(i => i.ShippingAddress, a =>
            {
                a.Property(p => p.Street).HasColumnName("ShippingStreet");
                a.Property(p => p.City).HasColumnName("ShippingCity");
                a.Property(p => p.State).HasColumnName("ShippingState");
                a.Property(p => p.ZipCode).HasColumnName("ShippingZipCode");
                a.Property(p => p.Country).HasColumnName("ShippingCountry");
            });
    }

    private void ConfigureCompanies(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(company =>
        {
            company.OwnsMany(c => c.Addresses, a =>
            {
                a.WithOwner().HasForeignKey("CompanyId"); // shadow FK
                a.Property<int>("Id"); // shadow key for EF tracking
                a.HasKey("Id"); // EF needs a PK, even for owned collections

                a.Property(p => p.Street).IsRequired();
                a.Property(p => p.City).IsRequired();
                a.Property(p => p.State).IsRequired();
                a.Property(p => p.ZipCode).IsRequired();
                a.Property(p => p.Country).IsRequired();
                a.Property(p => p.Type).HasConversion<string>();
            });
        });
    }

    public override int SaveChanges()
    {
        //ApplyAuditInfo();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var now = DateTime.UtcNow;
        var currentUserId = Guid.NewGuid(); // TODO: plug in user context/session

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedById = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedById = currentUserId;
                    break;

                default:
                    continue;
            }
        }
    }
}
