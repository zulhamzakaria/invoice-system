using InvoiceSystem.API.Endpoints;
using InvoiceSystem.Application.Mappers;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Application.Services;
using InvoiceSystem.Application.Services.Helpers.CompanyHelpers;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Repositories;
using InvoiceSystem.Infrastructure;
using InvoiceSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"),
    npgsqlOptions => npgsqlOptions.MigrationsAssembly("InvoiceSystem.Infrastructure")
    )
);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddScoped<IWorkflowstepService, WorkflowstepService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<ILoadTrackerService, LoadTrackerService>();
builder.Services.AddScoped<IInvoiceOrchestratorService, InvoiceOrchestratorService>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IWorkflowStepRepository, WorkflowStepRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ILoadTrackerRepository, LoadTrackerRepository>();

builder.Services.AddScoped<ICompanyMappingService, CompanyMappingService>();

builder.Services.AddScoped<IAddressMapper, AddressMapper>();
builder.Services.AddScoped<ICompanyMapper, CompanyMapper>();
builder.Services.AddScoped<IEmployeeMapper, EmployeeMapper>();
builder.Services.AddScoped<IInvoiceMapper, InvoiceMappper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    ////calling data seeder
    //using var scope = app.Services.CreateScope();
    //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //DataSeeder.Seed(dbContext);

}

app.UseHttpsRedirection();
app.UseAuthorization();

//Minimal API extensions
app.MapWorkflowstepEndpoints();
app.MapEmployeeEndpoints();

app.MapControllers();

app.Run();
