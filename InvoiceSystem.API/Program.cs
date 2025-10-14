using InvoiceSystem.API.Endpoints;
using InvoiceSystem.Application.Services;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Repositories;
using InvoiceSystem.Infrastructure;
using InvoiceSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"),
    npgsqlOptions => npgsqlOptions.MigrationsAssembly("InvoiceSystem.Infrastructure")
    )
);

builder.Services.AddScoped<IWorkflowstepService, WorkflowstepService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IWorkflowStepRepository, WorkflowStepRepository>();

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

app.MapControllers();

app.Run();
